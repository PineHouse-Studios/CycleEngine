using System.Collections.Generic;
using CycleEngine.Entities;

namespace CycleEngine.Definitions
{
    public class DeltaFrame
    {
        /// <summary>
        /// Copies of new values for entity that changed during the frame.
        /// </summary>
        public List<AnimatableEntity> EntityChanges { get; private set; }

        public DeltaFrame()
        {
            EntityChanges = new List<AnimatableEntity>();
        }

        public DeltaFrame(List<AnimatableEntity> changes)
        {
            EntityChanges = new List<AnimatableEntity>(changes);
        }

        internal void AppendChange(AnimatableEntity entity)
        {
            for (int i = 0; i < EntityChanges.Count; i++)
            {
                if (EntityChanges[i].EntityKey.Equals(entity.EntityKey))
                {
                    EntityChanges[i] = AnimatableEntity.Copy(entity);
                    return;
                }
            }
            EntityChanges.Add(AnimatableEntity.Copy(entity));
        }
        
        public bool IsEmpty => EntityChanges.Count == 0;
    }
}