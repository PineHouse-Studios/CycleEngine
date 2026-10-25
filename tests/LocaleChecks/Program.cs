// Run: dotnet run --project tests/LocaleChecks --framework net9.0
using CycleEngine.Core;
using CycleEngine.Services;
using CycleEngine.Utils;

var checks = 0;
var storage = new MemoryStorage();
var locale = new LocaleManager(storage);
Reject<ArgumentNullException>(() => new LocaleManager(null!));
Reject<ArgumentException>(() => locale.RegisterScript("", "mnz-1-1", "/zh.cds"));
Reject<ArgumentException>(() => locale.RegisterScript("zh", " ", "/zh.cds"));
Reject<ArgumentException>(() => locale.RegisterScript("zh", "mnz-1-1", null!));
Reject<ArgumentException>(() => locale.LoadScript(null!, "mnz-1-1"));
Reject<ArgumentException>(() => locale.LoadScript("zh", ""));
Reject<ArgumentException>(() => locale.Get(" "));

storage.Files["/zh.cds"] = """
    # Comments are ignored.
    [mnz-1-1:1]
    title = 小明
    text = 你好。

    [mnz-1-1:2]
    text = 旁白

    [mnz-1-1:3]
    - 好的
    - 不好
    """;
storage.Files["/en.cds"] = "[mnz-1-1:1]\ntitle = Ming\ntext = Hello.";
storage.Files["/next.cds"] = "[mnz-1-2:1]\ntext = Next chapter.";
Assert(locale.RegisterScript("zh", "mnz-1-1", "/zh.cds"), "Register Chinese");
Assert(locale.RegisterScript("en", "mnz-1-1", "/en.cds"), "Register English for the same script");
Assert(locale.RegisterScript("zh", "mnz-1-2", "/next.cds"), "Register another script for the same language");
Assert(!locale.RegisterScript("zh", "mnz-1-1", "/missing.cds"), "Duplicate registration must not overwrite path");
Assert(storage.ReadCount == 0, "Registration must not read files");
Assert(!locale.LoadScript("missing-language", "mnz-1-1"), "Unknown language returns false");
Assert(!locale.LoadScript("zh", "missing-script"), "Unknown script returns false");
Assert(!locale.LoadScript("ZH", "mnz-1-1"), "IDs use ordinal case-sensitive comparison");
Assert(storage.ReadCount == 0, "Unknown registration must not read files");

Assert(locale.LoadScript("zh", "mnz-1-1") && storage.ReadCount == 1, "Load only requested file");
Assert(locale.Get("mnz-1-1:1").Title == "小明" && locale.Get("mnz-1-1:1").Text == "你好。", "Dialogue properties");
Assert(locale.Get("mnz-1-1:2").Title is null && locale.Get("mnz-1-1:2").Text == "旁白", "Narration has no speaker");
Assert(locale.Get("mnz-1-1:3").Options!.SequenceEqual(new[] { "好的", "不好" }), "Choices exclude CDS syntax markers");
var choices = locale.Get("mnz-1-1:3");
choices.Options![0] = "changed";
Assert(locale.Get("mnz-1-1:3").Options![0] == "好的", "Returned choices cannot mutate loaded entries");
Assert(storage.ReadCount == 1, "Get reads loaded text without file I/O");
Reject<CycleResourceNotFoundException>(() => locale.Get("unknown"));

Assert(locale.RegisterScript("zh", "missing-file", "/absent.cds"), "Register missing file without reading");
Assert(!locale.LoadScript("zh", "missing-file"), "Missing file returns false");
Assert(locale.Get("mnz-1-1:1").Text == "你好。", "Missing file preserves loaded text");
Assert(locale.LoadScript("en", "mnz-1-1"), "Switch language");
Assert(locale.Get("mnz-1-1:1").Text == "Hello.", "English replaces Chinese entry");
Reject<CycleResourceNotFoundException>(() => locale.Get("mnz-1-1:2"));
Assert(locale.LoadScript("zh", "mnz-1-2"), "Switch chapter");
Reject<CycleResourceNotFoundException>(() => locale.Get("mnz-1-1:1"));
Assert(locale.Get("mnz-1-2:1").Text == "Next chapter.", "New chapter replaces previous chapter");

locale.RegisterScript("zh", "bad", "/bad.cds");
foreach (var invalid in new[]
{
    "text = no header", "[bad]\ntext = invalid ID", "[0000:1:1]\ntext = legacy three-part ID",
    "[chapter:]\ntext = empty event", "[:event]\ntext = empty chapter",
    "[chapter:event:extra]\ntext = too many parts", "[chapter name:event]\ntext = whitespace in chapter",
    "[chapter:event name]\ntext = whitespace in event", "[[chapter:event]\ntext = bracket in chapter",
    "[chapter:event]]\ntext = bracket in event",
    "[mnz-1-1:1]\ntext = first\n[mnz-1-1:1]\ntext = duplicate",
    "[mnz-1-1:1]\ntext = first\ntext = duplicate", "[mnz-1-1:1]\ntitle = first\ntitle = duplicate",
    "[mnz-1-1:1]\ntext = first\n- choice\ntext = duplicate",
    "[mnz-1-1:1]\n- ", "[mnz-1-1:1]", "[mnz-1-1:1]\n[mnz-1-1:2]\ntext = empty previous section",
    "[mnz-1-1:1]\nunknown line", "[mnz-1-1:1]\ntext = valid\n[0000:bad:2]"
})
{
    storage.Files["/bad.cds"] = invalid;
    var exception = Reject<CycleSyntaxException>(() => locale.LoadScript("zh", "bad"));
    Assert(exception.Message.Contains("/bad.cds:", StringComparison.Ordinal), "Syntax error includes path and line");
    Assert(locale.Get("mnz-1-2:1").Text == "Next chapter.", "Malformed file preserves loaded text");
}
storage.Files["/bad.cds"] = "[mnz-1-1:1]\ntext = valid\n[bad]";
Assert(Reject<CycleSyntaxException>(() => locale.LoadScript("zh", "bad")).Message.Contains("/bad.cds:3:", StringComparison.Ordinal), "Diagnostic line is one-based");
storage.ThrowOnRead = "/bad.cds";
Reject<IOException>(() => locale.LoadScript("zh", "bad"));
Assert(locale.Get("mnz-1-2:1").Text == "Next chapter.", "Storage failure preserves loaded text");
storage.ThrowOnRead = null;

locale.RegisterScript("zh", "mixed", "/mixed.cds");
foreach (var source in new[]
{
    "[mixed:1]\ntitle = Guide\ntext = Prompt\nContinuation\n- First\n- Second",
    "[mixed:1]\n- First\n- Second\ntitle = Guide\ntext = Prompt\nContinuation",
    "[mixed:1]\n- First\ntitle = Guide\ntext = Prompt\nContinuation\n- Second"
})
{
    storage.Files["/mixed.cds"] = source;
    Assert(locale.LoadScript("zh", "mixed"), "Properties and choices may coexist in any order");
    var entry = locale.Get("mixed:1");
    Assert(entry.Title == "Guide", "Mixed section preserves title");
    Assert(entry.Text == "Prompt\nContinuation", "Mixed section preserves multiline text");
    Assert(entry.Options!.SequenceEqual(new[] { "First", "Second" }), "Mixed section preserves ordered choices");
}

storage.Files["/format.cds"] = "\uFEFF# comment\r\n[format:1]\r\n  title = Main\r\nSubtitle\r\ntext = First  \r\n\r\n# comment in multiline text\r\nSecond\\nThird\r\n\r\n[format:2]\r\ntitle =\r\ntext =\r\n";
locale.RegisterScript("zh", "format", "/format.cds");
Assert(locale.LoadScript("zh", "format"), "BOM and CRLF input");
Assert(locale.Get("format:1").Title == "Main\nSubtitle", "Multiline speaker name");
Assert(locale.Get("format:1").Text == "First  \n\nSecond\nThird", "Multiline text preserves spaces, blank lines, and explicit newline escapes");
Assert(locale.Get("format:2").Title == "" && locale.Get("format:2").Text == "", "Explicit empty properties are valid");
storage.Files["/format.cds"] = "[format:1]\n- First\\nSecond\n- Last";
Assert(locale.LoadScript("zh", "format") && locale.Get("format:1").Options![0] == "First\nSecond", "Choice newline escapes");

storage.Files["/named.cds"] = "[mnz-1-1:Opening]\ntext = First chapter\n[mnz-1-2:Opening]\ntext = Second chapter\n[序章:开场]\ntext = Unicode IDs";
locale.RegisterScript("zh", "named", "/named.cds");
Assert(locale.LoadScript("zh", "named"), "Chapter and event IDs need not be numeric");
Assert(locale.Get("mnz-1-1:Opening").Text == "First chapter", "Hyphenated chapter and named event");
Assert(locale.Get("mnz-1-2:Opening").Text == "Second chapter", "Same event name in different chapters stays distinct");
Assert(locale.Get("序章:开场").Text == "Unicode IDs", "IDs support non-ASCII names");
Reject<CycleResourceNotFoundException>(() => locale.Get("mnz-1-1:opening"));
Reject<CycleResourceNotFoundException>(() => locale.Get("Opening"));

locale.UnloadAllText();
Reject<CycleResourceNotFoundException>(() => locale.Get("mnz-1-1:Opening"));
Assert(locale.LoadScript("zh", "mnz-1-1"), "Unload preserves registered paths");
locale.UnloadAllText();
locale.UnloadAllText();
Assert(locale.LoadScript("zh", "mnz-1-1"), "Repeated unload is safe");
storage.Files["/empty.cds"] = "# empty language pack\n\n";
locale.RegisterScript("zh", "empty", "/empty.cds");
Assert(locale.LoadScript("zh", "empty"), "Empty file is a valid zero-entry pack");
Reject<CycleResourceNotFoundException>(() => locale.Get("mnz-1-1:1"));
Reject<CycleResourceNotFoundException>(() => new LocaleManager(storage).Get("mnz-1-1:1"));

storage.Files["/sample.cds"] = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "sample.cds"));
locale.RegisterScript("zh", "sample", "/sample.cds");
Assert(locale.LoadScript("zh", "sample"), "Parse actual project CDS");
Assert(locale.Get("mnz-1-1:1").Title == "bob" && locale.Get("mnz-1-1:1").Text == "hi", "Actual Chapter:Event dialogue");
Assert(locale.Get("mnz-1-1:2").Title == "???" && locale.Get("mnz-1-1:2").Text == "...", "Actual anonymous speaker");
Reject<CycleResourceNotFoundException>(() => locale.Get("1"));

Console.WriteLine($"Locale checks passed ({checks} assertions), including the project's real CDS file.");

void Assert(bool condition, string message)
{
    if (!condition) throw new Exception(message);
    checks++;
}
T Reject<T>(Action action) where T : Exception
{
    try { action(); }
    catch (T exception) { checks++; return exception; }
    throw new Exception($"Expected {typeof(T).Name}.");
}

sealed class MemoryStorage : IStorageBackend
{
    public Dictionary<string, string> Files { get; } = new(StringComparer.Ordinal);
    public int ReadCount { get; private set; }
    public string? ThrowOnRead { get; set; }
    public string RootPath => "/";
    public string? ReadText(string relativePath)
    {
        ReadCount++;
        if (relativePath == ThrowOnRead) throw new IOException("Read failure.");
        return Files.TryGetValue(relativePath, out var source) ? source : null;
    }

    public bool Exists(string relativePath) => throw new NotSupportedException();
    public Task<string>? ReadTextAsync(string relativePath, CancellationToken ct = default) => throw new NotSupportedException();
    public Task<byte[]>? ReadBytesAsync(string relativePath, CancellationToken ct = default) => throw new NotSupportedException();
    public byte[]? ReadBytes(string relativePath, CancellationToken ct = default) => throw new NotSupportedException();
    public Task<Stream>? OpenReadAsync(string relativePath, CancellationToken ct = default) => throw new NotSupportedException();
    public Task WriteTextAsync(string relativePath, string content, CancellationToken ct = default) => throw new NotSupportedException();
    public Task WriteBytesAsync(string relativePath, byte[] content, CancellationToken ct = default) => throw new NotSupportedException();
    public void Delete(string relativePath) => throw new NotSupportedException();
    public IEnumerable<string>? ListFiles(string relativeDirectory = "", string? searchPattern = null, bool recursive = false) => throw new NotSupportedException();
    public string ResolveAbsolutePath(string relativePath) => throw new NotSupportedException();
}
