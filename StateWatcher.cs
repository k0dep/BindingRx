using System;
using UniRx;

namespace BindingRx
{
    /// <summary>
    ///     Watch for state of object and notify about changes
    /// </summary>
    /// <typeparam name="T"> type of target object type </typeparam>
    public class StateWatcher<T> : IWatcher
    {
        private object lastState;
        private T source;
        private Func<T, object> stateAccessor;

        /// <param name="source"> target object instance </param>
        /// <param name="stateAccessor"> delegate for access to target property of object </param>
        public StateWatcher(T source, Func<T, object> stateAccessor)
        {
            lastState = null;
            this.source = source;
            this.stateAccessor = stateAccessor;
        }
        
        public IObservable<object> Watch()
        {
            return Observable.EveryUpdate()
                .Select(_ => stateAccessor(source))
                .Where(cState => (cState == null && cState != lastState) || (cState != null && !cState.Equals(lastState)))
                .Select(cState => lastState = cState);
        }

        public void UpdateState()
        {
            lastState = stateAccessor(source);
        }
    }
}