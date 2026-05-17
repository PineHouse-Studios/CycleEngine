using System.Collections.Generic;
using CycleEngine.Commands;

namespace CycleEngine.Definitions
{
    public class ScriptBlock
    {
        public Command[] Commands { set; get; } = null!;
        
        public Dictionary<string, ScriptBlock> NamedBlocks { set; get; } = null!;
    }
}