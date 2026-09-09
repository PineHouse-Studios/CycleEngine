using CycleEngine.Definitions;

namespace CycleEngine.Entities
{
    public class Music(string entityKey)
        : Entity(entityKey)
    {
        public override Entity Copy()
        {
            throw new System.NotImplementedException();
        }
    }
}