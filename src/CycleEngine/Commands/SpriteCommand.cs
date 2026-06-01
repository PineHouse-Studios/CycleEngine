using System.Collections.Generic;
using System.Threading.Tasks;
using CycleEngine.Definitions;

namespace CycleEngine.Commands
{
    public class SpriteCommand : Command
    {
        public SpriteCommand(List<Token> parameters) : base(parameters)
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