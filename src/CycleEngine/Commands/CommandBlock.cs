using System.Collections.Generic;
using CycleEngine.Definitions;
using CycleEngine.Utils;

namespace CycleEngine.Commands
{
    public abstract class CommandBlock : Command
    {
        protected CommandBlock(List<Token> parameters, Command[] body, int lineNumber) : base(parameters, lineNumber)
        {
            Body = body;
        }
        public Command[] Body { set; get; }
    }
}