using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CycleEngine.Definitions;
using CycleEngine.Entities;
using CycleEngine.Utils;

namespace CycleEngine.Core
{
    public class Animator
    {
        private CycleEngine _engine;

        public Animator(CycleEngine engine)
        {
            _engine = engine;
        }

        private readonly List<Tween<AnimatableEntity>> ActiveTween;

        public void Update(EngineTime time)
        {
            foreach (var e in ActiveTween)
            {
            }
        }

        public DeltaFrame CollectFrame()
        {
            throw new NotImplementedException();
        }

        public void RegisterTween(AnimatableEntity targetEntity, AnimatableEntity endingState)
        {
            ActiveTween.Add(new Tween<AnimatableEntity>
            {
                Begin = AnimatableEntity.Copy(targetEntity),
                Current = targetEntity,
                End = endingState
            });
        }

        public void CancelAll()
        {
            throw new NotImplementedException();
        }
    }
}