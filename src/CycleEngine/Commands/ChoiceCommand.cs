using System.Collections.Generic;
using System.Threading.Tasks;
using CycleEngine.Definitions;
using CycleEngine.Interpreter;
using CycleEngine.Utils;

namespace CycleEngine.Commands
{
    public class ChoiceCommand : CommandBlock
    {
        public ChoiceCommand(CycleEngine engine, List<Token> parameters, int lineNumber, Command[] body) : base(engine, parameters, lineNumber, body)
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