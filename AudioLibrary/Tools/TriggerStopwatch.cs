using System;
using System.Diagnostics;
using AudioAnalysisLibrary.Interfaces;

namespace AudioAnalysisLibrary.Tools
{
    public class TriggerStopwatch : ITimer
    {
        private Stopwatch SystemStopwatch { get; }

        public TimeSpan Elapsed => SystemStopwatch.Elapsed;

        public long ElapsedMilliseconds => SystemStopwatch.ElapsedMilliseconds;

        public long ElapsedTicks => SystemStopwatch.ElapsedTicks;

        public bool IsRunning => SystemStopwatch.IsRunning;

        public TriggerStopwatch()
        {
            SystemStopwatch = new Stopwatch();
        }

        public void Start()
        {
            SystemStopwatch.Start();
        }

        public void Stop()
        {
            SystemStopwatch.Stop();
        }

        public void Restart()
        {
            SystemStopwatch.Restart();
        }

        public void Reset()
        {
            SystemStopwatch.Reset();
        }
    }
}
