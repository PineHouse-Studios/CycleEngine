using System.Collections.Generic;
using CycleEngine.Definitions;
using CycleEngine.Entities;

namespace CycleEngine.Core
{
    public abstract class Animator<T> where T : Entity
    {
        internal DeltaFrame<T> CurFrame = new DeltaFrame<T>();
        internal readonly List<Tween<T>> ActiveTween = new List<Tween<T>>();
        internal readonly List<Tween<T>> PausedTween = new List<Tween<T>>();
        internal abstract void Update(EngineTime time);

        public DeltaFrame<T> CollectFrame()
        {
            DeltaFrame<T> frame = new DeltaFrame<T>(CurFrame.EntityChanges);
            CurFrame = new DeltaFrame<T>();
            return frame;
        }

        public void Register(T targetEntity, T endingState)
        {
            ActiveTween.Add(new Tween<T>
            {
                Begin = (T) targetEntity.Copy(),
                Current = targetEntity,
                End = endingState
            });
        }
        public abstract void CompleteAll();
        public abstract bool Complete(string entityKey);
        public void PauseAll()
        {
            foreach (var e in ActiveTween)
            {
                PausedTween.Add(e);
            }
            ActiveTween.Clear();
        }

        public bool Pause(string entityKey)
        {
            for (int i = 0; i < ActiveTween.Count; i++)
            {
                if (ActiveTween[i].Current.EntityKey.Equals(entityKey))
                {
                    PausedTween.Add(ActiveTween[i]);
                    ActiveTween.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public void ResumeAll()
        {
            foreach (var e in PausedTween)
            {
                ActiveTween.Add(e);
            }
            PausedTween.Clear();
        }
        
        public bool Resume(string entityKey)
        {
            for (int i = 0; i < PausedTween.Count; i++)
            {
                if (PausedTween[i].Current.EntityKey.Equals(entityKey))
                {
                    ActiveTween.Add(PausedTween[i]);
                    PausedTween.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
    }
}