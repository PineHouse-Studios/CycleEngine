using System.Collections.Generic;
using CycleEngine.Definitions;
using CycleEngine.Entities;
using CycleEngine.Utils;

namespace CycleEngine.Core
{
    public class Animator
    {
        private CycleEngine _engine;
        private DeltaFrame _curFrame = new DeltaFrame();

        public Animator(CycleEngine engine)
        {
            _engine = engine;
        }

        private readonly List<Tween<AnimatableEntity>> _activeTween = new List<Tween<AnimatableEntity>>();

        public void Update(EngineTime time)
        {
            for (int i = 0; i < _activeTween.Count; i++)
            {
                Tween<AnimatableEntity> e = _activeTween[i];
                e.TimeElapsed += time.DeltaTime;
                bool tweenFinished = true;

                if (e.TimeElapsed < e.End.X.Duration)
                {
                    e.Current.X.Value = Easing.Ease(Easing.GetType(e.End.X.EaseType), e.TimeElapsed,
                        e.Begin.X.Value, e.End.X.Value, e.End.X.Duration);
                    tweenFinished = false;
                }
                else
                {
                    e.Current.X.Value = e.End.X.Value;
                }
                
                if (e.TimeElapsed < e.End.Y.Duration)
                {
                    e.Current.Y.Value = Easing.Ease(Easing.GetType(e.End.Y.EaseType), e.TimeElapsed,
                        e.Begin.Y.Value, e.End.Y.Value, e.End.Y.Duration);
                    tweenFinished = false;
                }
                else
                {
                    e.Current.Y.Value = e.End.Y.Value;
                }
                
                if (e.TimeElapsed < e.End.Rotation.Duration)
                {
                    e.Current.Rotation.Value = Easing.Ease(Easing.GetType(e.End.Rotation.EaseType), e.TimeElapsed,
                        e.Begin.Rotation.Value, e.End.Rotation.Value, e.End.Rotation.Duration);
                    tweenFinished = false;
                }
                else
                {
                    e.Current.Rotation.Value = e.End.Rotation.Value;
                }
                
                
                if (e.TimeElapsed < e.End.Zoom.Duration)
                {
                    e.Current.Zoom.Value = Easing.Ease(Easing.GetType(e.End.Zoom.EaseType), e.TimeElapsed,
                        e.Begin.Zoom.Value, e.End.Zoom.Value, e.End.Zoom.Duration);
                    tweenFinished = false;
                }
                else
                {
                    e.Current.Zoom.Value = e.End.Zoom.Value;
                }
                
                if (e.TimeElapsed < e.End.Alpha.Duration)
                {
                    e.Current.Alpha.Value = Easing.Ease(Easing.GetType(e.End.Alpha.EaseType), e.TimeElapsed,
                        e.Begin.Alpha.Value, e.End.Alpha.Value, e.End.Alpha.Duration);
                    tweenFinished = false;
                }
                else
                {
                    e.Current.Alpha.Value = e.End.Alpha.Value;
                }

                if (tweenFinished)
                {
                    _activeTween.RemoveAt(i);
                    i--;
                }
                
                _curFrame.AppendChange(e.Current);
            }
        }

        public DeltaFrame CollectFrame()
        {
            DeltaFrame frame = new DeltaFrame(_curFrame.EntityChanges);
            _curFrame = new DeltaFrame();
            return frame;
        }

        public void RegisterTween(AnimatableEntity targetEntity, AnimatableEntity endingState)
        {
            _activeTween.Add(new Tween<AnimatableEntity>
            {
                Begin = AnimatableEntity.Copy(targetEntity),
                Current = targetEntity,
                End = endingState
            });
        }

        public void CompleteAll()
        {
            foreach (var e in _activeTween)
            {
                e.Current.X.Value = e.End.X.Value;
                e.Current.Y.Value = e.End.Y.Value;
                e.Current.Rotation.Value = e.End.Rotation.Value;
                e.Current.Zoom.Value = e.End.Zoom.Value;
                e.Current.Alpha.Value = e.End.Alpha.Value;
                _curFrame.AppendChange(e.Current);
            }
            _activeTween.Clear();
        }
    }
}