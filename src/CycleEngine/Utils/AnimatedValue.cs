namespace CycleEngine.Utils
{
    public class AnimatedValue<T>
    {
        public T Value { get; set; } = default!;
        public double Duration { get; set; }
        public string Ease { get; set; } = "linear";
    }

    public class AnimatedVelocityValue<T> : AnimatedValue<T>
    {
        public AnimatedValue<double>? Velocity { get; set; }
    }
}