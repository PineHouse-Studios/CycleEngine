using System.Threading.Tasks;

namespace CycleEngine.Commands
{
    public interface ICommand
    {
        public abstract Task AsyncExecute();
        public void ForceComplete();
    }
}