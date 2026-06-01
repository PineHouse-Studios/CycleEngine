using CycleEngine.Definitions;

namespace CycleEngine.Entities
{
    public abstract class Entity
    {
        public string EntityKey { get; set; }

        public Entity(string entityKey)
        {
            EntityKey = entityKey;
        }

        public abstract Entity Copy();
    }
}