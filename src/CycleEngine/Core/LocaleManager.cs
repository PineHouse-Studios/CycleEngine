using System;
using System.Collections.Generic;
using CycleEngine.Definitions;
using CycleEngine.Services;
using CycleEngine.Utils;

namespace CycleEngine.Core;

public class LocaleManager : IService
{
    private readonly Dictionary<string, Dictionary<string, string>> CdsList = new(StringComparer.Ordinal);
    private readonly Dictionary<string, CycleText> Loaded = new(StringComparer.Ordinal);
    private readonly IStorageBackend _storage;

    public LocaleManager(IStorageBackend storage)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    /// <summary>Registers a path without reading the file. Returns false if already registered.</summary>
    public bool RegisterScript(string language, string scriptId, string path)
    {
        RequireValue(language, nameof(language));
        RequireValue(scriptId, nameof(scriptId));
        RequireValue(path, nameof(path));

        if (!CdsList.TryGetValue(language, out var scripts))
        {
            scripts = new Dictionary<string, string>(StringComparer.Ordinal);
            CdsList.Add(language, scripts);
        }

        if (scripts.ContainsKey(scriptId)) return false;
        scripts.Add(scriptId, path);
        return true;
    }

    /// <summary>Loads one CDS file. Returns false for an unregistered or missing file.</summary>
    /// <exception cref="CycleSyntaxException">The CDS file is malformed; existing text remains loaded.</exception>
    public bool LoadScript(string language, string scriptId)
    {
        RequireValue(language, nameof(language));
        RequireValue(scriptId, nameof(scriptId));

        if (!CdsList.TryGetValue(language, out var scripts) || !scripts.TryGetValue(scriptId, out var path))
            return false;

        var source = _storage.ReadText(path);
        if (source is null) return false;

        // Parse the entire new file before discarding the currently loaded language/chapter.
        var parsed = CdsParser.Parse(source, path);
        Loaded.Clear();
        foreach (var entry in parsed)
            Loaded.Add(entry.Key, entry.Value);
        return true;
    }

    /// <summary>Gets a loaded CDS entry by its complete Chapter:Event ID, for example mnz-1-1:1.</summary>
    public CycleText Get(string eventId)
    {
        RequireValue(eventId, nameof(eventId));
        if (!Loaded.TryGetValue(eventId, out var text))
            throw new CycleResourceNotFoundException(eventId);

        // The struct is copied by value, but its option array would otherwise remain shared.
        if (text.Options is not null) text.Options = (string[])text.Options.Clone();
        return text;
    }

    public void UnloadAllText() => Loaded.Clear();

    private static void RequireValue(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("A non-empty value is required.", parameterName);
    }
}
