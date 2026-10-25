using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CycleEngine.Definitions;
using CycleEngine.Utils;

namespace CycleEngine.Core;

internal static class CdsParser
{
    private static readonly Regex Header = new(@"^\[([^:\[\]\s]+:[^:\[\]\s]+)\]$", RegexOptions.CultureInvariant);
    private static readonly Regex Property = new(@"^(title|text)\s*=\s*(.*)$", RegexOptions.CultureInvariant);

    public static Dictionary<string, CycleText> Parse(string source, string path)
    {
        var entries = new Dictionary<string, CycleText>(StringComparer.Ordinal);
        var properties = new HashSet<string>(StringComparer.Ordinal);
        var options = new List<string>();
        string? id = null;
        string? title = null;
        string? text = null;
        string? currentProperty = null;
        var lineNumber = 0;
        var sectionLine = 0;

        CycleSyntaxException Error(string message, int line) => new($"{path}:{line}: {message}");

        void Append(string value)
        {
            if (currentProperty == "title") title += "\n" + value;
            if (currentProperty == "text") text += "\n" + value;
        }

        void FinishSection()
        {
            if (id is null) return;
            if (properties.Count == 0 && options.Count == 0)
                throw Error("CDS section cannot be empty.", sectionLine);
            entries.Add(id, new CycleText
            {
                Title = title?.TrimEnd('\n').Replace("\\n", "\n"),
                Text = text?.TrimEnd('\n').Replace("\\n", "\n"),
                Options = options.Count == 0 ? null : options.ToArray()
            });
        }

        using var reader = new StringReader(source.TrimStart('\uFEFF'));
        string? rawLine;
        while ((rawLine = reader.ReadLine()) is not null)
        {
            lineNumber++;
            var line = rawLine.TrimStart();
            if (line.StartsWith("#", StringComparison.Ordinal)) continue;
            if (string.IsNullOrWhiteSpace(line))
            {
                Append(string.Empty);
                continue;
            }

            var trimmed = line.TrimEnd();
            if (trimmed.StartsWith("[", StringComparison.Ordinal))
            {
                var header = Header.Match(trimmed);
                if (!header.Success)
                    throw Error("Expected a CDS section ID in [Chapter:Event] format, such as [mnz-1-1:1].", lineNumber);
                FinishSection();
                id = header.Groups[1].Value;
                if (entries.ContainsKey(id))
                    throw Error($"Duplicate CDS section ID: {id}.", lineNumber);
                sectionLine = lineNumber;
                title = text = currentProperty = null;
                properties.Clear();
                options.Clear();
                continue;
            }

            if (id is null)
                throw Error("Content must follow a CDS section header.", lineNumber);

            var property = Property.Match(line);
            if (property.Success)
            {
                var name = property.Groups[1].Value;
                if (!properties.Add(name))
                    throw Error($"Duplicate CDS property: {name}.", lineNumber);
                if (name == "title") title = property.Groups[2].Value;
                else text = property.Groups[2].Value;
                currentProperty = name;
                continue;
            }

            if (line.StartsWith("-", StringComparison.Ordinal))
            {
                var option = line.Substring(1).Trim();
                if (option.Length == 0)
                    throw Error("CDS choice cannot be empty.", lineNumber);
                options.Add(option.Replace("\\n", "\n"));
                currentProperty = null;
                continue;
            }

            if (currentProperty is null)
                throw Error("Expected title, text, or a choice starting with -.", lineNumber);
            Append(line);
        }

        FinishSection();
        return entries;
    }
}
