using System;

namespace AudioAnalysisLibrary.Interfaces
{
    /// <summary>
    /// An interface that allows for adapting system stopwatch and enables
    /// a mock to be created for testing.
    /// </summary>
    public interface ITimer
    {
        TimeSpan Elapsed { get; }

        long ElapsedMilliseconds { get; }

        long ElapsedTicks { get; }

        bool IsRunning { get; }

        void Start();

        void Stop();

        void Restart();

        void Reset();
    }
}