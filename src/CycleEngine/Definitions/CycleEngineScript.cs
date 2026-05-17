using System.Collections.Generic;

namespace CycleEngine.Definitions
{
    public class CycleEngineScript
    {
        public int ScriptId { get; set; }
        public Dictionary<string, string> Attributes { set; get; } = null!;
        public Dictionary<string, ScriptBlock> Body { set; get; } = null!;
    }
}