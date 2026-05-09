using System;

namespace CycleEngine.Commands
{
    public abstract class CommandBlockBase : CommandBase
    {
        protected CommandBlockBase(string raw) : base(raw)
        {
            
        }
        public CommandBase[] Body { set; get; } = Array.Empty<CommandBase>();
    }
}