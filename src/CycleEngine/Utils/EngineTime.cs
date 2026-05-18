namespace CycleEngine.Utils
{
    public class EngineTime
    {
        /// <summary>
        /// Total milliseconds since the engine started.
        /// </summary>
        public double TotalTime { get; private set; }

        /// <summary>
        /// Milliseconds elapsed since the last Update call.
        /// </summary>
        public double DeltaTime { get; private set; }

        /// <summary>
        /// Time scale multiplier. 1.0 = normal, 2.0 = double speed, 0 = paused.
        /// </summary>
        public double TimeScale { get; set; } = 1.0;

        /// <summary>
        /// Frame counter, useful for debugging.
        /// </summary>
        public long FrameCount { get; private set; }

        internal void Advance(double rawDeltaMs)
        {
            DeltaTime = rawDeltaMs * TimeScale;
            TotalTime += DeltaTime;
            FrameCount++;
        }
    }
}