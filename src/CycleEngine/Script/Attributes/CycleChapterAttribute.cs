using System;

namespace CycleEngine.Script.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CycleChapterAttribute : Attribute
{
    public string? Id { get; }
    
    public static class ReservedId
    {
        public const string Initialization = "init";
    }

    public CycleChapterAttribute(string? id = null)
    {
        Id = id;
    }
}