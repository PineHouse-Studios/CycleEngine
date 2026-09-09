using System.Collections.Generic;
using CycleEngine.Definitions;
using CycleEngine.Entities;

namespace CycleEngine.Core
{
    public abstract class Animator<T> where T : Entity
    {
        private CycleEngine _engine;
        internal DeltaFrame<T> CurFrame = new();
        internal readonly List<Tween<T>> ActiveTween = new();
        internal readonly List<Tween<T>> PausedTween = new();

        public Animator(CycleEngine engine)
        {
            _engine = engine;
        }
        
        internal abstract void Update(EngineTime time);

        public DeltaFrame<T> CollectFrame()
        {
            DeltaFrame<T> frame = new(CurFrame.EntityChanges);
            CurFrame = new();
            return frame;
        }

        public bool ActiveContains(string entityKey)
        {
            foreach (var e in ActiveTween)
            {
                if (e.Current.EntityKey.Equals(entityKey)) return true;
            }

            return false;
        }

        public bool PausedContains(string entityKey)
        {
            foreach (var e in PausedTween)
            {
                if (e.Current.EntityKey.Equals(entityKey)) return true;
            }

            return false;
        }

        public void Register(T targetEntity, T endingState)
        {
            ActiveTween.Add(new()
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