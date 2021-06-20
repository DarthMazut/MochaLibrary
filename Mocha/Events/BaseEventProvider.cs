using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocha.Events
{
    /// <summary>
    /// Provides a base class for <see cref="IEventProvider{TEventArgs}"/> implementations.
    /// </summary>
    /// <typeparam name="TEventArgs">Type of arguments for encapsulated event.</typeparam>
    public abstract class BaseEventProvider<TEventArgs> : IEventProvider<TEventArgs> where TEventArgs : BaseEventArgs
    {
        /// <summary>
        /// A collection of <see cref="AsyncEventHandler{T}"/> event handlers, which are to be
        /// executed when encapsulated event occurs.
        /// </summary>
        protected List<AsyncEventHandler<TEventArgs>> AsyncInvocationList { get; } = new List<AsyncEventHandler<TEventArgs>>();

        /// <inheritdoc/>
        public abstract event EventHandler<TEventArgs> Event;

        /// <inheritdoc/>
        public virtual void SubscribeAsync(AsyncEventHandler<TEventArgs> asyncEventHandler)
        {
            AsyncInvocationList.Add(asyncEventHandler);
        }

        /// <inheritdoc/>
        public virtual void UnsubscribeAsync(Func<TEventArgs, IReadOnlyCollection<AsyncEventHandler>, Task> function)
        {
            int toRemove = AsyncInvocationList.FindIndex(h => h.Equals(function));
            AsyncInvocationList.RemoveAt(toRemove);
        }

        /// <summary>
        /// Returns a collection of <see cref="AsyncEventHandler{T}"/> event handlers in order of their execution priority.
        /// </summary>
        protected List<AsyncEventHandler<TEventArgs>> GetSortedInvocationList()
        {
            return AsyncInvocationList.Where(h => h.ExecuteInParallel == false).OrderBy(h => h.Priority).ToList();
        }

        /// <summary>
        /// Starts execution and returns collection of event handlers with <see cref="AsyncEventHandler.ExecuteInParallel"/>
        /// flag set to <see langword="true"/> for encapsulated event.
        /// </summary>
        /// <param name="eventArgs">Common arguments for execution of parallel event handlers.</param>
        protected List<Task> StartAndGetParallelCollection(TEventArgs eventArgs)
        {
            return AsyncInvocationList.Where(h => h.ExecuteInParallel).Select(h => h.Execute(eventArgs, AsyncInvocationList.AsReadOnly())).ToList();
        }
    }
}
