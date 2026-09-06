using System.Collections.Generic;
using CycleEngine.Definitions;
using CycleEngine.Interpreter;
using CycleEngine.Utils;

namespace CycleEngine.Commands
{
    public abstract class CommandBlock : Command
    {
        protected CommandBlock(CycleEngine engine, List<Token> parameters, int lineNumber, Command[] body) : base(engine, parameters, lineNumber)
        {
            Body = body;
        }

        public Command[] Body { set; get; }
    }
}