using System.Collections.Generic;
using CycleEngine.Entities;

namespace CycleEngine.Definitions
{
    public class CycleEngineScript
    {
        public Dictionary<string, string> Attributes { set; get; } = null!;
        public Dictionary<string, ScriptBlock> Body { set; get; } = null!;
    }
}