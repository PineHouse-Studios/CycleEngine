using System.Collections.Generic;
using CycleEngine.Entities;

namespace CycleEngine.Definitions
{
    public class DeltaFrame<T> where T : Entity
    {
        /// <summary>
        /// Copies of new values for entity that changed during the frame.
        /// </summary>
        public List<T> EntityChanges { get; private set; }

        public DeltaFrame()
        {
            EntityChanges = new List<T>();
        }

        public DeltaFrame(List<T> changes)
        {
            EntityChanges = new List<T>(changes);
        }

        internal void Update(T entity)
        {
            for (int i = 0; i < EntityChanges.Count; i++)
            {
                if (EntityChanges[i].EntityKey.Equals(entity.EntityKey))
                {
                    EntityChanges[i] = (T) entity.Copy();
                    return;
                }
            }
            EntityChanges.Add((T) entity.Copy());
        }
        
        public bool IsEmpty => EntityChanges.Count == 0;
    }
}