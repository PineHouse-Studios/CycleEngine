using System;

namespace CycleEngine.Script.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CycleEventAttribute : Attribute
{
    public string? Id { get; }
    public bool AutoRun { get; set; } = true;

    public static class ReservedId
    {
        public const string OnLoad = "onload";
        public const string OnExit = "onexit";
    }

    public CycleEventAttribute(string? id = null)
    {
        Id = id;
    }
}