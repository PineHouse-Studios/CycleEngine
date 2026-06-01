using CycleEngine.Definitions;

namespace CycleEngine.Entities
{
    public class Video : Entity
    {
        public Video(string entityKey) : base(entityKey)
        {
        }

        public override Entity Copy()
        {
            throw new System.NotImplementedException();
        }
    }
}