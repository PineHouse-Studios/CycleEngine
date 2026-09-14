using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

if (args.Contains("--self-test"))
{
    var sample = """
        [0000:1:1]
        title = bob
        text = 你好

        [0000:1:2]
        - 好的
        - 不好
        """;
    var result = CdsParser.Parse(sample);
    if (result.Diagnostics.Count != 0 || result.Sections.Count != 2)
        throw new Exception("CDS parser self-test failed.");

    var invalid = CdsParser.Parse("text = missing section\n[bad]\n[0000:1:1]\n");
    if (invalid.Diagnostics.Count != 3)
        throw new Exception("CDS parser diagnostic self-test failed.");
    if (CdsParser.NextSectionId(sample, 7) != "[0000:1:3]" || CdsParser.NextSectionId("", 0) != "[0000:1:1]")
        throw new Exception("CDS completion self-test failed.");
    if (CdsParser.Parse("[0000:1:1]\ntitle =\ntext =").Diagnostics.Count != 0)
        throw new Exception("Empty CDS properties self-test failed.");
    var multiline = CdsParser.Parse("[0000:1:1]\ntitle = Main\nSubtitle\ntext = First  \nSecond\\nThird");
    if (multiline.Diagnostics.Count != 0 || multiline.Sections[0].Title != "Main\nSubtitle" ||
        LanguageServer.SectionPreview(multiline.Sections[0]) != "### **Main**\n### **Subtitle**\n\nFirst  \nSecond  \nThird")
        throw new Exception("Multiline CDS preview self-test failed.");

    Console.Error.WriteLine("CDS parser self-test passed.");
    return;
}

await new LanguageServer(Console.OpenStandardInput(), Console.OpenStandardOutput()).RunAsync();

internal sealed class LanguageServer(Stream input, Stream output)
{
    private readonly Stream _input = input;
    private readonly Stream _output = output;
    private readonly Dictionary<string, string> _documents = [];

    public async Task RunAsync()
    {
        while (await ReadMessageAsync() is { } message)
        {
            var method = message["method"]?.GetValue<string>();
            var id = message["id"]?.DeepClone();

            try
            {
                switch (method)
                {
                    case "initialize":
                        await ReplyAsync(id, new
                        {
                            capabilities = new
                            {
                                textDocumentSync = 1,
                                hoverProvider = true,
                                documentSymbolProvider = true,
                                foldingRangeProvider = true,
                                completionProvider = new { triggerCharacters = new[] { "[", "t" } }
                            },
                            serverInfo = new { name = "CDS Language Server", version = "0.1.8" }
                        });
                        break;
                    case "shutdown":
                        await ReplyAsync(id, null);
                        break;
                    case "exit":
                        return;
                    case "textDocument/didOpen":
                    {
                        var document = message["params"]!["textDocument"]!;
                        await UpdateAsync(document["uri"]!.GetValue<string>(), document["text"]!.GetValue<string>());
                        break;
                    }
                    case "textDocument/didChange":
                    {
                        var parameters = message["params"]!;
                        var uri = parameters["textDocument"]!["uri"]!.GetValue<string>();
                        var changes = parameters["contentChanges"]!.AsArray();
                        await UpdateAsync(uri, changes[^1]!["text"]!.GetValue<string>());
                        break;
                    }
                    case "textDocument/didClose":
                    {
                        var uri = message["params"]!["textDocument"]!["uri"]!.GetValue<string>();
                        _documents.Remove(uri);
                        await NotifyAsync("textDocument/publishDiagnostics", new { uri, diagnostics = Array.Empty<object>() });
                        break;
                    }
                    case "textDocument/documentSymbol":
                    {
                        var uri = message["params"]!["textDocument"]!["uri"]!.GetValue<string>();
                        var sections = CdsParser.Parse(_documents.GetValueOrDefault(uri, "")).Sections;
                        await ReplyAsync(id, sections.Select(section => new
                        {
                            name = section.Id,
                            detail = "CDS section",
                            kind = 2,
                            range = Range(section.Line, 0, section.EndLine, section.EndCharacter),
                            selectionRange = Range(section.Line, 0, section.Line, section.Id.Length + 2)
                        }));
                        break;
                    }
                    case "textDocument/foldingRange":
                    {
                        var uri = message["params"]!["textDocument"]!["uri"]!.GetValue<string>();
                        var sections = CdsParser.Parse(_documents.GetValueOrDefault(uri, "")).Sections;
                        await ReplyAsync(id, sections.Where(section => section.EndLine > section.Line).Select(section => new
                        {
                            startLine = section.Line,
                            endLine = section.EndLine,
                            kind = "region",
                            collapsedText = section.Id
                        }));
                        break;
                    }
                    case "textDocument/hover":
                        await ReplyAsync(id, Hover(message));
                        break;
                    case "textDocument/completion":
                        await ReplyAsync(id, Completion(message));
                        break;
                    default:
                        if (id is not null)
                            await ErrorAsync(id, -32601, $"Method not found: {method}");
                        break;
                }
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
                if (id is not null)
                    await ErrorAsync(id, -32603, exception.Message);
            }
        }
    }

    private async Task UpdateAsync(string uri, string text)
    {
        _documents[uri] = text;
        await NotifyAsync("textDocument/publishDiagnostics", new
        {
            uri,
            diagnostics = CdsParser.Parse(text).Diagnostics.Select(diagnostic => new
            {
                range = Range(diagnostic.Line, diagnostic.Start, diagnostic.Line, diagnostic.End),
                severity = diagnostic.Severity,
                source = "cds",
                message = diagnostic.Message
            })
        });
    }

    private object? Hover(JsonNode message)
    {
        var parameters = message["params"]!;
        var uri = parameters["textDocument"]!["uri"]!.GetValue<string>();
        var line = parameters["position"]!["line"]!.GetValue<int>();
        var lines = _documents.GetValueOrDefault(uri, "").Replace("\r", "").Split('\n');
        if (line >= lines.Length) return null;

        var value = lines[line].Trim();
        var section = value.StartsWith('[')
            ? CdsParser.Parse(_documents.GetValueOrDefault(uri, "")).Sections.FirstOrDefault(item => item.Line == line)
            : null;
        var description = section is not null ? SectionPreview(section)
            : value.StartsWith('[') ? "Section ID: `chapter:scene:item`"
            : value.StartsWith("title") ? "Speaker name"
            : value.StartsWith("text") ? "Dialogue text"
            : value.StartsWith('-') ? "Player choice"
            : null;
        return description is null ? null : new { contents = new { kind = "markdown", value = description } };
    }

    internal static string SectionPreview(Section section)
    {
        var parts = new List<string>();
        var title = section.Title?.TrimEnd('\n');
        var text = section.Text?.TrimEnd('\n');
        if (!string.IsNullOrEmpty(title))
            parts.Add(string.Join('\n', title.Replace("\\n", "\n").Split('\n').Select(line => line.Length == 0 ? "" : $"### **{line}**")));
        if (!string.IsNullOrEmpty(text)) parts.Add(text.Replace("\\n", "  \n"));
        if (section.Choices.Count > 0) parts.Add(string.Join('\n', section.Choices).Replace("\\n", "  \n"));
        return parts.Count > 0 ? string.Join("\n\n", parts) : "Empty section";
    }

    private object Completion(JsonNode message)
    {
        var parameters = message["params"]!;
        var uri = parameters["textDocument"]!["uri"]!.GetValue<string>();
        var position = parameters["position"]!;
        var lineNumber = position["line"]!.GetValue<int>();
        var character = position["character"]!.GetValue<int>();
        var source = _documents.GetValueOrDefault(uri, "");
        var lines = source.Replace("\r", "").Split('\n');
        var line = lineNumber < lines.Length ? lines[lineNumber] : "";
        var prefix = line[..Math.Min(character, line.Length)];
        var bracket = prefix.LastIndexOf('[');

        if (bracket >= 0 && prefix[(bracket + 1)..].All(value => char.IsDigit(value) || value == ':'))
        {
            var next = CdsParser.NextSectionId(source, lineNumber);
            var end = character < line.Length && line[character] == ']' ? character + 1 : character;
            return new[]
            {
                new
                {
                    label = next,
                    kind = 15,
                    detail = "Next CDS section",
                    insertTextFormat = 2,
                    textEdit = new
                    {
                        range = Range(lineNumber, bracket, lineNumber, end),
                        newText = $"{next}\ntitle = $1\ntext = $2"
                    }
                }
            };
        }

        var wordStart = prefix.Length;
        while (wordStart > 0 && char.IsLetter(prefix[wordStart - 1])) wordStart--;
        var word = prefix[wordStart..];
        if (word.Length > 0 && prefix[..wordStart].All(char.IsWhiteSpace) &&
            CdsParser.HasSectionBefore(source, lineNumber) &&
            ("text".StartsWith(word, StringComparison.Ordinal) || "title".StartsWith(word, StringComparison.Ordinal)))
        {
            var range = Range(lineNumber, wordStart, lineNumber, character);
            return new[]
            {
                new { label = "text", kind = 10, textEdit = new { range, newText = "text = " } },
                new { label = "title", kind = 10, textEdit = new { range, newText = "title = " } }
            };
        }

        return Array.Empty<object>();
    }

    private async Task<JsonNode?> ReadMessageAsync()
    {
        var header = new List<byte>();
        while (true)
        {
            var next = _input.ReadByte();
            if (next < 0) return null;
            header.Add((byte)next);
            if (header.Count >= 4 && header[^4] == '\r' && header[^3] == '\n' && header[^2] == '\r' && header[^1] == '\n')
                break;
        }

        var headerText = Encoding.ASCII.GetString(header.ToArray());
        var lengthLine = headerText.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault(line => line.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase));
        if (lengthLine is null || !int.TryParse(lengthLine.AsSpan("Content-Length:".Length).Trim(), out var length))
            throw new InvalidDataException("Missing Content-Length header.");

        var body = new byte[length];
        var read = 0;
        while (read < length)
        {
            var count = await _input.ReadAsync(body.AsMemory(read, length - read));
            if (count == 0) throw new EndOfStreamException();
            read += count;
        }
        return JsonNode.Parse(body);
    }

    private Task ReplyAsync(JsonNode? id, object? result) =>
        WriteAsync(new { jsonrpc = "2.0", id, result });

    private Task ErrorAsync(JsonNode id, int code, string message) =>
        WriteAsync(new { jsonrpc = "2.0", id, error = new { code, message } });

    private Task NotifyAsync(string method, object @params) =>
        WriteAsync(new { jsonrpc = "2.0", method, @params });

    private async Task WriteAsync(object message)
    {
        var body = JsonSerializer.SerializeToUtf8Bytes(message);
        var header = Encoding.ASCII.GetBytes($"Content-Length: {body.Length}\r\n\r\n");
        await _output.WriteAsync(header);
        await _output.WriteAsync(body);
        await _output.FlushAsync();
    }

    private static object Range(int startLine, int startCharacter, int endLine, int endCharacter) => new
    {
        start = new { line = startLine, character = startCharacter },
        end = new { line = endLine, character = endCharacter }
    };
}

internal static partial class CdsParser
{
    [GeneratedRegex(@"^\[(\d{4}):(\d+):(\d+)\]\s*$")]
    private static partial Regex HeaderPattern();

    [GeneratedRegex(@"^(title|text)\s*=\s*(.*)$")]
    private static partial Regex PropertyPattern();

    public static string NextSectionId(string source, int beforeLine)
    {
        var match = PreviousSection(source, beforeLine);
        if (match.Success && long.TryParse(match.Groups[3].Value, out var item) && item < long.MaxValue)
            return $"[{match.Groups[1].Value}:{match.Groups[2].Value}:{item + 1}]";
        return "[0000:1:1]";
    }

    public static bool HasSectionBefore(string source, int beforeLine) => PreviousSection(source, beforeLine).Success;

    private static Match PreviousSection(string source, int beforeLine)
    {
        var lines = source.Replace("\r", "").Split('\n');
        for (var line = Math.Min(beforeLine - 1, lines.Length - 1); line >= 0; line--)
        {
            var match = HeaderPattern().Match(lines[line].Trim());
            if (match.Success) return match;
        }
        return Match.Empty;
    }

    public static ParseResult Parse(string source)
    {
        var diagnostics = new List<Diagnostic>();
        var sections = new List<Section>();
        var ids = new HashSet<string>();
        Section? current = null;
        string? currentProperty = null;
        var keys = new HashSet<string>();
        var hasChoice = false;
        var lines = source.Replace("\r", "").Split('\n');

        void FinishSection(int endLine)
        {
            if (current is null) return;
            current.EndLine = Math.Max(current.Line, endLine);
            current.EndCharacter = lines[current.EndLine].Length;
            if (keys.Count == 0 && !hasChoice)
                diagnostics.Add(new(current.Line, 0, current.Id.Length + 2, 2, "Section cannot be empty."));
        }

        for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
        {
            var line = lines[lineNumber];
            var trimmed = line.Trim();
            var syntaxLine = line.TrimStart();
            if (trimmed.StartsWith('#')) continue;
            if (trimmed.Length == 0)
            {
                current?.Append(currentProperty, "");
                continue;
            }

            var header = HeaderPattern().Match(trimmed);
            if (header.Success)
            {
                FinishSection(lineNumber - 1);
                var id = header.Value[1..^1];
                if (!ids.Add(id))
                    diagnostics.Add(new(lineNumber, 0, line.Length, 1, $"Duplicate section ID: {id}"));
                current = new(id, lineNumber, lineNumber, line.Length);
                sections.Add(current);
                currentProperty = null;
                keys.Clear();
                hasChoice = false;
                continue;
            }

            if (current is null)
            {
                diagnostics.Add(new(lineNumber, 0, line.Length, 1,
                    trimmed.StartsWith('[') ? "Section header must use [chapter:scene:item], for example [0000:1:1]." : "Content must follow a section header."));
                continue;
            }

            var property = PropertyPattern().Match(syntaxLine);
            if (property.Success)
            {
                var key = property.Groups[1].Value;
                if (hasChoice)
                    diagnostics.Add(new(lineNumber, 0, line.Length, 1, "A section cannot mix properties and choices."));
                if (!keys.Add(key))
                    diagnostics.Add(new(lineNumber, 0, key.Length, 1, $"Property {key} is duplicated."));
                if (key == "title") current.Title = property.Groups[2].Value;
                else current.Text = property.Groups[2].Value;
                currentProperty = key;
                continue;
            }

            if (syntaxLine.StartsWith('-'))
            {
                if (keys.Count > 0)
                    diagnostics.Add(new(lineNumber, 0, line.Length, 1, "A section cannot mix properties and choices."));
                if (syntaxLine[1..].Trim().Length == 0)
                    diagnostics.Add(new(lineNumber, 0, line.Length, 2, "Choice cannot be empty."));
                hasChoice = true;
                currentProperty = null;
                current.Choices.Add(syntaxLine);
                continue;
            }

            if (currentProperty is not null)
            {
                current.Append(currentProperty, syntaxLine);
                continue;
            }

            diagnostics.Add(new(lineNumber, 0, line.Length, 1, "Unrecognized line; use title, text, or a choice starting with -."));
        }

        FinishSection(lines.Length - 1);
        return new(diagnostics, sections);
    }
}

internal sealed record ParseResult(List<Diagnostic> Diagnostics, List<Section> Sections);
internal sealed record Diagnostic(int Line, int Start, int End, int Severity, string Message);
internal sealed record Section(string Id, int Line, int EndLine, int EndCharacter)
{
    public int EndLine { get; set; } = EndLine;
    public int EndCharacter { get; set; } = EndCharacter;
    public string? Title { get; set; }
    public string? Text { get; set; }
    public List<string> Choices { get; } = [];

    public void Append(string? property, string value)
    {
        if (property == "title") Title += $"\n{value}";
        if (property == "text") Text += $"\n{value}";
    }
}
