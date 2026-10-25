using CycleEngine.Definitions;
using CycleEngine.Entities;
using CycleEngine.Services;
using CycleEngine.Utils;

namespace CycleEngine.Animation
{
    public class AnimatableEntityAnimator(CycleEngine engine)
        : Animator<AnimatableEntity>(engine), IService
    {
        internal override void Update(EngineTime time)
        {
            for (int i = 0; i < ActiveTween.Count; i++)
            {
                AnimatableEntity e = ActiveTween[i];
                e.Alpha.TimeElapsed += time.DeltaTime;
                e.Rotation.TimeElapsed += time.DeltaTime;
                e.X.TimeElapsed += time.DeltaTime;
                e.Y.TimeElapsed += time.DeltaTime;
                e.Zoom.TimeElapsed += time.DeltaTime;
                
                bool tweenFinished = true;

                if (e.X.TimeElapsed < e.X.Duration)
                {
                    e.X.Value = Easing.Ease(Easing.GetType(e.X.EaseType), e.X.TimeElapsed,
                        e.X.FromValue, e.X.ToValue, e.X.Duration);
                    tweenFinished = false;
                }
                else
                {
                    e.X.Value = e.X.ToValue;
                }
                
                if (e.Y.TimeElapsed < e.Y.Duration)
                {
                    e.Y.Value = Easing.Ease(Easing.GetType(e.Y.EaseType), e.Y.TimeElapsed,
                        e.Y.FromValue, e.Y.ToValue, e.Y.Duration);
                    tweenFinished = false;
                }
                else
                {
                    e.Y.Value = e.Y.ToValue;
                }
                
                if (e.Rotation.TimeElapsed < e.Rotation.Duration)
                {
                    e.Rotation.Value = Easing.Ease(Easing.GetType(e.Rotation.EaseType), e.Rotation.TimeElapsed,
                        e.Rotation.FromValue, e.Rotation.ToValue, e.Rotation.Duration);
                    tweenFinished = false;
                }
                else
                {
                    e.Rotation.Value = e.Rotation.ToValue;
                }
                
                
                if (e.Zoom.TimeElapsed < e.Zoom.Duration)
                {
                    e.Zoom.Value = Easing.Ease(Easing.GetType(e.Zoom.EaseType), e.Zoom.TimeElapsed,
                        e.Zoom.FromValue, e.Zoom.ToValue, e.Zoom.Duration);
                    tweenFinished = false;
                }
                else
                {
                    e.Zoom.Value = e.Zoom.ToValue;
                }
                
                if (e.Alpha.TimeElapsed < e.Alpha.Duration)
                {
                    e.Alpha.Value = Easing.Ease(Easing.GetType(e.Alpha.EaseType), e.Alpha.TimeElapsed,
                        e.Alpha.FromValue, e.Alpha.ToValue, e.Alpha.Duration);
                    tweenFinished = false;
                }
                else
                {
                    e.Alpha.Value = e.Alpha.ToValue;
                }

                if (tweenFinished)
                {
                    ActiveTween.RemoveAt(i);
                    i--;
                }
                
                CurFrame.Update(e);
            }
        }

        public override void CompleteAll()
        {
            foreach (var e in ActiveTween)
            {
                e.X.Value = e.X.ToValue;
                e.Y.Value = e.Y.ToValue;
                e.Rotation.Value = e.Rotation.ToValue;
                e.Zoom.Value = e.Zoom.ToValue;
                e.Alpha.Value = e.Alpha.ToValue;
                CurFrame.Update(e);
            }
            ActiveTween.Clear();
        }

        public override bool Complete(string entityKey)
        {
            for (int i = 0; i < ActiveTween.Count; i ++)
            {
                if (ActiveTween[i].EntityKey.Equals(entityKey))
                {
                    ActiveTween[i].X.Value = ActiveTween[i].X.ToValue;
                    ActiveTween[i].Y.Value = ActiveTween[i].Y.ToValue;
                    ActiveTween[i].Rotation.Value = ActiveTween[i].Rotation.ToValue;
                    ActiveTween[i].Zoom.Value = ActiveTween[i].Zoom.ToValue;
                    ActiveTween[i].Alpha.Value = ActiveTween[i].Alpha.ToValue;
                    CurFrame.Update(ActiveTween[i]);
                    ActiveTween.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
    }
}