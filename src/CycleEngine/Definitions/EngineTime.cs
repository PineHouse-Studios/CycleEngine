namespace CycleEngine.Definitions
{
    public class EngineTime
    {
        /// <summary>
        /// Total milliseconds since the engine started.
        /// </summary>
        public double TotalTime { get; private set; }
        
        /// <summary>
        /// Milliseconds elapsed since last event
        /// </summary>
        public double TimeElapsed { get; private set;}

        /// <summary>
        /// Milliseconds elapsed since the last Update call.
        /// </summary>
        public double DeltaTime { get; private set; }

        /// <summary>
        /// Timescale multiplier. 1.0 = normal, 2.0 = double speed, 0 = paused.
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
            TimeElapsed += DeltaTime;
            FrameCount++;
        }

        internal void ResetTimeElapsed()
        {
            TimeElapsed = 0d;
        }
    }
}