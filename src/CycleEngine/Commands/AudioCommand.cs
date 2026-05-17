using System.Collections.Generic;
using System.Threading.Tasks;
using CycleEngine.Utils;

namespace CycleEngine.Commands
{
    public class AudioCommand : Command
    {
        public AudioCommand(List<Token> parameters) : base(parameters)
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