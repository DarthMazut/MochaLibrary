using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MochaCore.Events
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
        protected List<AsyncEventHandler<TEventArgs>> AsyncInvocationList { get; } = new();

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
        /// Additionally sets <see cref="AsyncEventHandler.Executed"/> flag to <see langword="false"/> for all returned handlers.
        /// </summary>
        protected List<AsyncEventHandler<TEventArgs>> GetSortedInvocationList()
        {
            List<AsyncEventHandler<TEventArgs>>? sortedInvocationList = AsyncInvocationList
                .Where(h => h.ExecuteInParallel == false)
                .OrderByDescending(h => h.Priority)
                .ToList();

            sortedInvocationList.ForEach(h => h.Executed = false);

            return sortedInvocationList;
        }

        /// <summary>
        /// Executes one by one all stored <see cref="AsyncEventHandler"/> objects in order of their execution priortiy
        /// with <see cref="AsyncEventHandler.ExecuteInParallel"/> flag set to <see langword="false"/>. 
        /// </summary>
        /// <param name="eventArgs">Event arguments shared by all event handlers.</param>
        protected async Task ExecuteAllNonParallel(TEventArgs eventArgs)
        {
            foreach (AsyncEventHandler<TEventArgs>? handler in GetSortedInvocationList())
            {
                await handler.Execute(eventArgs, AsyncInvocationList.AsReadOnly());
            }
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
