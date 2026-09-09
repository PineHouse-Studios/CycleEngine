using CycleEngine.Definitions;
using CycleEngine.Entities;
using CycleEngine.Services;
using CycleEngine.Utils;

namespace CycleEngine.Core
{
    public class AnimatableEntityAnimator(CycleEngine engine)
        : Animator<AnimatableEntity>(engine), IService
    {
        internal override void Update(EngineTime time)
        {
            for (int i = 0; i < ActiveTween.Count; i++)
            {
                Tween<AnimatableEntity> e = ActiveTween[i];
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
                    ActiveTween.RemoveAt(i);
                    i--;
                }
                
                CurFrame.Update(e.Current);
            }
        }

        public override void CompleteAll()
        {
            foreach (var e in ActiveTween)
            {
                e.Current.X.Value = e.End.X.Value;
                e.Current.Y.Value = e.End.Y.Value;
                e.Current.Rotation.Value = e.End.Rotation.Value;
                e.Current.Zoom.Value = e.End.Zoom.Value;
                e.Current.Alpha.Value = e.End.Alpha.Value;
                CurFrame.Update(e.Current);
            }
            ActiveTween.Clear();
        }

        public override bool Complete(string entityKey)
        {
            for (int i = 0; i < ActiveTween.Count; i ++)
            {
                if (ActiveTween[i].Current.EntityKey.Equals(entityKey))
                {
                    ActiveTween[i].Current.X.Value = ActiveTween[i].End.X.Value;
                    ActiveTween[i].Current.Y.Value = ActiveTween[i].End.Y.Value;
                    ActiveTween[i].Current.Rotation.Value = ActiveTween[i].End.Rotation.Value;
                    ActiveTween[i].Current.Zoom.Value = ActiveTween[i].End.Zoom.Value;
                    ActiveTween[i].Current.Alpha.Value = ActiveTween[i].End.Alpha.Value;
                    CurFrame.Update(ActiveTween[i].Current);
                    ActiveTween.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
    }
}