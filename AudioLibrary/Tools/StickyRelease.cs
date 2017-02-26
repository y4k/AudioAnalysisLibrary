using System;
using System.Collections.Generic;
using AudioAnalysisLibrary.Interfaces;

namespace AudioAnalysisLibrary.Tools
{
    /// <summary>
    /// StickyRelease will begin when a value is greater than its start value and
    /// will cease when the value is lower than its stop value for a given period
    /// of time. It will reset the timer when the value goes back above the start
    /// value.
    /// </summary>
    public class StickyRelease<TCompare> where TCompare : IComparable
    {
        //Actions to trigger on start and stop.
        public event Action<bool> Active;

        public TCompare StartValue { get; }
        public TCompare StopValue { get; }

        private bool _running;
        private bool _active;
        private readonly ITimer _stopwatch;
        private readonly TimeSpan _timeInterval;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="startValue">The value at which to become active</param>
        /// <param name="stopValue">The value at which to become inactive</param>
        /// <param name="timeInterval">The TimeSpan that the update values must stay below the stop threshold to deactivate.</param>
        /// <param name="timer">Timer</param>
        public StickyRelease(TCompare startValue, TCompare stopValue, TimeSpan timeInterval, ITimer timer = null)
        {
            if (startValue.CompareTo(stopValue) < 0)
            {
                throw new ArgumentException("Stop value must be less than the Start value.");
            }

            _stopwatch = timer == null ? _stopwatch = new TriggerStopwatch() : _stopwatch = timer;

            StartValue = startValue;
            StopValue = stopValue;

            _timeInterval = timeInterval;
        }

        /// <summary>
        /// Overload of basic constuctor. Takes an additonal parameter which is an Action that is automatically
        /// subscribed to the event that is triggered when the StickyRelease changes state.
        /// </summary>
        /// <param name="startValue">The value at which to become active</param>
        /// <param name="stopValue">The value at which to become inactive</param>
        /// <param name="triggerAction">Action that expects a bool and is triggered when the StickyRelease changes state</param>
        /// <param name="timeInterval">The TimeSpan that the stopwatch will wait until it deactivates if the update value remains below the stop value.</param>
        public StickyRelease(TCompare startValue, TCompare stopValue, TimeSpan timeInterval, Action<bool> triggerAction) : this(startValue, stopValue, timeInterval)
        {
            Active += triggerAction;
        }

        /// <summary>
        /// Overload of basic constuctor. Takes an additonal parameter which is an IEnumerable of Action where each
        /// Action is is automatically subscribed to the event that is triggered when the StickyRelease changes state.
        /// </summary>
        /// <param name="startValue">The value at which to become active</param>
        /// <param name="stopValue">The value at which to become inactive</param>
        /// <param name="triggerActions">IEnumerable of Actions that expect a bool and triggered when the StickyRelease changes state</param>
        /// /// <param name="timeInterval">The TimeSpan that the stopwatch will wait until it deactivates if the update value remains below the stop value.</param>
        public StickyRelease(TCompare startValue, TCompare stopValue, TimeSpan timeInterval, IEnumerable<Action<bool>> triggerActions) : this(startValue, stopValue, timeInterval)
        {
            foreach (var action in triggerActions)
            {
                Active += action;
            }
        }

        public bool Start()
        {
            //Set running
            _running = true;
            //Set active to false
            _active = false;
            //Stop the timer
            _stopwatch.Stop();
            return _running;
        }

        public bool Stop()
        {
            //Stop running
            _running = false;
            //Deactivate
            _active = false;
            //Stop the timer
            _stopwatch.Stop();
            return _running;
        }

        public void Update(TCompare updateValue)
        {
            //Does nothing if not running
            if (!_running)
            {
                return;
            }

            if (updateValue == null)
            {
                return;
            }

            //If active, compare against the stop value...
            if (_active)
            {
                //If the new value is lower than the stop value and timer has exceeded the required interval , deactivate
                if (updateValue.CompareTo(StopValue) < 0 && _stopwatch.Elapsed > _timeInterval)
                {
                    Deactivate();
                }
                //If the value is not below the stop value, restart the timer.
                else
                {
                    _stopwatch.Restart();
                }
            }
            //...otherwise if inactive, compare against start value.
            else
            {
                //If the new value is greater than the start value, activate
                if (updateValue.CompareTo(StartValue) >= 0)
                {
                    Activate();
                }
            }
        }

        protected void Activate()
        {
            //Set to active
            _active = true;
            //Set the timer to zero and start
            _stopwatch.Restart();
            OnActive(_active);
        }

        protected void Deactivate()
        {
            //Set to inactive
            _active = false;
            //Reset the timer and stop it.
            _stopwatch.Stop();
            OnActive(_active);
        }

        protected virtual void OnActive(bool active)
        {
            Active?.Invoke(active);
        }
    }
}
