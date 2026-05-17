using System.Collections.Generic;
using CycleEngine.Utils;

namespace CycleEngine.Commands
{
    public abstract class CommandBlock : Command
    {
        protected CommandBlock(List<Token> parameters, Command[] body) : base(parameters)
        {
            Body = body;
        }
        public Command[] Body { set; get; }
    }
}