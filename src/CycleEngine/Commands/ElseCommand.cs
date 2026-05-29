using System.Collections.Generic;
using System.Threading.Tasks;
using CycleEngine.Definitions;
using CycleEngine.Utils;

namespace CycleEngine.Commands
{
    public class ElseCommand : CommandBlock
    {
        public ElseCommand(List<Token> parameters, Command[] body) : base(parameters, body)
        {
        }

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