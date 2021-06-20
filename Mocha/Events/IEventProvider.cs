using Mocha.Events.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mocha.Events
{
    /// <summary>
    /// Encapsulates single event which can be subscribed to.
    /// </summary>
    public interface IEventProvider<TEventArgs> where TEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Encapsulated event.
        /// </summary>
        event EventHandler<TEventArgs> Event;

        /// <summary>
        /// Registers <see cref="AsyncEventHandler{T}"/> object which will be executed whenever encapsulated event occurs.
        /// </summary>
        /// <param name="asyncEventHandler">Object to be registered.</param>
        void SubscribeAsync(AsyncEventHandler<TEventArgs> asyncEventHandler);

        /// <summary>
        /// Removes given function from subscribtion collection.
        /// </summary>
        /// <param name="function">Function to be unsubscribed.</param>
        void UnsubscribeAsync(Func<TEventArgs, IReadOnlyCollection<AsyncEventHandler>, Task> function);
    }
}