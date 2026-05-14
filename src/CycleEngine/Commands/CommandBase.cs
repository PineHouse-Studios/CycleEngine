using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CycleEngine.Core;
using CycleEngine.Interpreter;
using CycleEngine.Utils;

namespace CycleEngine.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected CommandBase(string raw)
        {
            
        }
        
        protected int LineNumber { get; set; }
        protected List<Token> Parameter { get; }
        
        public abstract Task AsyncExecute();
        public abstract void ForceComplete();
    }
}