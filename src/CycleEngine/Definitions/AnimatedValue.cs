using System;

namespace CycleEngine.Definitions
{
    public class AnimatedValue<T>
    {
        public T FromValue { get; set; } = default!;
        public T Value { get; set; } = default!;
        public T ToValue { get; set; } = default!;
        public double Duration { get; set; }
        public double TimeElapsed { set; get; }
        public string EaseType { get; set; } = "linear";

        public static AnimatedValue<T> Copy(AnimatedValue<T> av)
        {
            return new()
            {
                TimeElapsed = av.TimeElapsed,
                Duration = av.Duration,
                FromValue = av.FromValue,
                Value = av.Value,
                ToValue = av.ToValue,
                EaseType = av.EaseType
            };
        }
    }

    public class AnimatedVelocityValue<T> : AnimatedValue<T>
    {
        public AnimatedValue<double>? Velocity { get; set; }
        public double Time { set; get; }

        public static AnimatedVelocityValue<T> Copy(AnimatedVelocityValue<T> avv)
        {
            return new()
            {
                TimeElapsed = avv.TimeElapsed,
                Duration = avv.Duration,
                FromValue = avv.FromValue,
                Value = avv.Value,
                ToValue = avv.ToValue,
                EaseType = avv.EaseType,
                Velocity = avv.Velocity is null ? null : AnimatedValue<double>.Copy(avv.Velocity),
                Time = avv.Time
            };
        }
    }
}