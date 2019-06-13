using System;

namespace BindingRx
{
    /// <summary>
    ///     Entity what watch for changes in object
    /// </summary>
    public interface IWatcher
    {
        /// <summary>
        ///     Start watching
        /// </summary>
        /// <returns> subscribable observer what indicate changes </returns>
        IObservable<object> Watch();
        
        /// <summary>
        ///     Will stimulate update local state of watcher instance
        /// </summary>
        void UpdateState();
    }
}