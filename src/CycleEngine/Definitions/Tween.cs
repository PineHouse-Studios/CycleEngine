namespace CycleEngine.Definitions
{
    public class Tween<T>
    {
        public T Begin = default!;
        public T Current = default!;
        public T End = default!;
        public double TimeElapsed = 0;
    }
}