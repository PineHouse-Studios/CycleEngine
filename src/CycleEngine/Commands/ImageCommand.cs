using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CycleEngine.Core;
using CycleEngine.Definitions;
using CycleEngine.Interpreter;
using CycleEngine.Utils;

namespace CycleEngine.Commands
{
    public class ImageCommand : Command
    {
        public ImageCommand(List<Token> parameters, int lineNumber) : base(parameters, lineNumber)
        {
            Operation = Parameter[1].Value;
            EntityKey = Parameter[2].Value;
            
            for (int i = 1; i < Parameter.Count; i++)
            {
                // 
            }
        }

        public string Operation { get; private set; }
        public string EntityKey { get; private set; }
        public string? TargetContentKey { get; set; }
        public int? TargetX { get; set; }
        public int? TargetY { get; set; }
        public int? TargetZoom { get; set; }
        public int? TargetRot { get; set; }
        public int? In { get; set; }
        public int? Out { get; set; }
        public bool? TargetShow { get; set; }
        
        
        public override Task AsyncExecute()
        {
            throw new System.NotImplementedException();
        }

        public override void ForceComplete()
        {
            throw new System.NotImplementedException();
        }
    }
}