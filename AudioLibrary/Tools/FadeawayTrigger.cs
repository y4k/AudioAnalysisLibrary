using System;
using System.Collections.Generic;

namespace AudioAnalysisLibrary.Tools
{
    /// <summary>
    /// Becomes active when it receives a value higer than a start parameter.
    /// Will continue to be active whilst the value remains above a stop parameter, lower than the start parameter.
    /// Once it dips below the stop parameter, become inactive.
    /// </summary>
    public class FadeawayTrigger<TCompare> where TCompare : IComparable
    {
        public event Action<bool> StateChange;

        public TCompare StartValue { get; }
        public TCompare StopValue { get; }
        public bool IsRunning { get; private set; }

        private bool _active;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="startValue">The value at which it becomes active.</param>
        /// <param name="stopValue">The value below which it becomes inactive.</param>
        public FadeawayTrigger(TCompare startValue, TCompare stopValue)
        {
            if (startValue.CompareTo(stopValue) < 0)
            {
                throw new ArgumentException("Stop value must be less than the Start value.");
            }

            StartValue = startValue;
            StopValue = stopValue;
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// /// <param name="startValue">The value at which it becomes active.</param>
        /// <param name="stopValue">The value below which it becomes inactive.</param>
        /// <param name="triggerAction">An action that is automatically subscribed to the StateChange event.</param>
        public FadeawayTrigger(TCompare startValue, TCompare stopValue, Action<bool> triggerAction) : this(startValue, stopValue)
        {
            StateChange += triggerAction;
        }

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// /// <param name="startValue">The value at which it becomes active.</param>
        /// <param name="stopValue">The value below which it becomes inactive.</param>
        /// <param name="triggerActions">An IEnumerable of actions that are automatically subscribed to the StateChange event.</param>
        public FadeawayTrigger(TCompare startValue, TCompare stopValue, IEnumerable<Action<bool>> triggerActions) : this(startValue, stopValue)
        {
            foreach (var action in triggerActions)
            {
                StateChange += action;
            }
        }

        public void Start()
        {
            IsRunning = true;
            _active = false;
        }

        public void Stop()
        {
            IsRunning = false;
            _active = false;
        }

        public bool Update(TCompare updateValue)
        {
            if (!IsRunning)
            {
                return false;
            }

            //If running and update value is lower than stop value 
            if (_active && updateValue.CompareTo(StopValue) < 0)
            {
                _active = false;
                OnStateChange(_active);
            }
            //If not active and update value is greater than start value
            else if(!_active && updateValue.CompareTo(StartValue) >= 0)
            {
                _active = true;
                OnStateChange(_active);
            }

            return true;
        }

        protected virtual void OnStateChange(bool active)
        {
            StateChange?.Invoke(active);
        }
    }
}
