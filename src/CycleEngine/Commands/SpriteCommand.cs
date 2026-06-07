using System.Collections.Generic;
using System.Threading.Tasks;
using CycleEngine.Definitions;

namespace CycleEngine.Commands
{
    public class SpriteCommand : Command
    {
        public SpriteCommand(CycleEngine engine, List<Token> parameters, int lineNumber) : base(engine, parameters, lineNumber)
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