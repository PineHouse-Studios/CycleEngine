using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CycleEngine.Script.Types;

public class CycleScriptConfig
{
    public CycleScriptConfig(string chapterId, params KeyValuePair<string, Action>[] events)
    {
        if (string.IsNullOrWhiteSpace(chapterId)) throw new ArgumentException("Chapter ID is required.", nameof(chapterId));
        if (events is null) throw new ArgumentNullException(nameof(events));

        ChapterId = chapterId;
        var order = new string[events.Length];
        var registry = new Dictionary<string, Action>(StringComparer.Ordinal);
        for (var index = 0; index < events.Length; index++)
        {
            var entry = events[index];
            if (string.IsNullOrWhiteSpace(entry.Key)) throw new ArgumentException("Event ID is required.", nameof(events));
            if (entry.Value is null) throw new ArgumentException("Event handler is required.", nameof(events));
            registry.Add(entry.Key, entry.Value);
            order[index] = entry.Key;
        }

        ExecutionOrder = Array.AsReadOnly(order);
        EventRegistry = new ReadOnlyDictionary<string, Action>(registry);
    }

    public string ChapterId { get; }
    public IReadOnlyList<string> ExecutionOrder { get; }
    public IReadOnlyDictionary<string, Action> EventRegistry { get; }
    public int EventCount => ExecutionOrder.Count;
}
