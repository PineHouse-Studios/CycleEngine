using System.Collections.Generic;
using System.Threading.Tasks;
using CycleEngine.Utils;

namespace CycleEngine.Commands
{
    public class IfCommand : CommandBlock
    {
        public IfCommand(List<Token> parameters, Command[] body) : base(parameters, body)
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