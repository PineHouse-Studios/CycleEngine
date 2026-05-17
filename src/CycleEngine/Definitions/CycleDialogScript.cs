using System.Collections.Generic;
using CycleEngine.Entities;

namespace CycleEngine.Definitions
{
    public class CycleDialogScript
    {
        public Dictionary<string, string> Attributes { set; get; } = null!;
        public Dictionary<string, Dialog> Body { set; get; } = null!;
    }
}