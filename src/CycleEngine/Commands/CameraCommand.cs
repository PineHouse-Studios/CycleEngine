using System.Collections.Generic;
using System.Threading.Tasks;
using CycleEngine.Definitions;
using CycleEngine.Utils;

namespace CycleEngine.Commands
{
    public class CameraCommand : Command
    {
        public CameraCommand(CycleEngine engine, List<Token> parameters, int lineNumber) : base(engine, parameters, lineNumber)
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