using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CycleEngine.Core;
using CycleEngine.Interpreter;
using CycleEngine.Utils;

namespace CycleEngine.Commands
{
    public abstract class Command : ICommand
    {
        public Command(List<Token> parameters)
        {
            Parameter = parameters;
        }
        
        protected int LineNumber { get; set; }
        protected List<Token> Parameter { get; }
        
        public abstract Task AsyncExecute();
        public abstract void ForceComplete();
    }
}