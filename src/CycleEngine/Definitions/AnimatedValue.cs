using System;

namespace CycleEngine.Definitions
{
    public class AnimatedValue<T>
    {
        public T Value { get; set; } = default!;
        public double Duration { get; set; }
        public string EaseType { get; set; } = "linear";

        public static AnimatedValue<T> Copy(AnimatedValue<T> av)
        {
            return new()
            {
                Duration = av.Duration,
                Value = av.Value,
                EaseType = av.EaseType
            };
        }
    }

    public class AnimatedVelocityValue<T> : AnimatedValue<T>
    {
        public AnimatedValue<double>? Velocity { get; set; }
        public T MinValue { get; set; } = default!;
        public T MaxValue { get; set; } = default!;
        
        public static AnimatedVelocityValue<T> Copy(AnimatedVelocityValue<T> avv)
        {
            return new()
            {
                Duration = avv.Duration,
                Value = avv.Value,
                EaseType = avv.EaseType,
                Velocity = avv.Velocity is null ? null : AnimatedValue<double>.Copy(avv.Velocity),
                MinValue = avv.MinValue,
                MaxValue = avv.MaxValue
            };
        }
    }
}