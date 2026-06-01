using CycleEngine.Definitions;

namespace CycleEngine.Entities
{
    public class Music : Entity
    {
        public Music(string entityKey) : base(entityKey)
        {
        }

        public override Entity Copy()
        {
            throw new System.NotImplementedException();
        }
    }
}