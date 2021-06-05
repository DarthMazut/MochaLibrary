using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mocha.Events
{
    /// <summary>
    /// Base type for <see cref="AsyncEventHandler{T}"/> class.
    /// </summary>
    public abstract class AsyncEventHandler
    {
        protected bool _executed;

        /// <summary>
        /// Determines whether execution of this <see cref="AsyncEventHandler"/> will be skipped in current iteration.
        /// </summary>
        public bool SkipCurrentIteration { get; set; }

        /// <summary>
        /// Determines whether this <see cref="AsyncEventHandler"/> has been already executed.
        /// </summary>
        public bool Executed => _executed;

        /// <summary>
        /// Identifier of <see cref="AsyncEventHandler"/>.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Determines whether this <see cref="AsyncEventHandler"/> can be executed in parallel together with
        /// other subscribed <see cref="AsyncEventHandler"/> objects.
        /// </summary>
        public bool ExecuteInParallel { get; }

        /// <summary>
        /// Determines order of this <see cref="AsyncEventHandler"/> execution. Lower priority means first to execute.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="id">Identifier of <see cref="AsyncEventHandler"/>.</param>
        /// <param name="priority">Determines order of execution.</param>
        /// <param name="executeInParallel">See <see cref="AsyncEventHandler.ExecuteInParallel"/> property.</param>
        public AsyncEventHandler(string id, int priority, bool executeInParallel)
        {
            Id = id;
            Priority = priority;
            ExecuteInParallel = executeInParallel;
        }
    }

    /// <summary>
    /// Encapsulates event handler which can be executed asynchronously.
    /// </summary>
    /// <typeparam name="T">Type of event arguments.</typeparam>
    public class AsyncEventHandler<T> : AsyncEventHandler where T : EventArgs
    {
        private const string DefaultID = "__MochaLibDefaultAsyncEventID__";
       
        private Func<T, IReadOnlyCollection<AsyncEventHandler>, Task> _asyncEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncEventHandler{T}"/> class.
        /// </summary>
        /// <param name="eventHandler">Delegate returning asynchronous event handler.</param>
        public AsyncEventHandler(Func<T, IReadOnlyCollection<AsyncEventHandler>, Task> eventHandler) : this(eventHandler, DefaultID, int.MaxValue, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncEventHandler{T}"/> class.
        /// </summary>
        /// <param name="eventHandler">Delegate returning asynchronous event handler.</param>
        /// <param name="id">Identifier of <see cref="AsyncEventHandler"/>.</param>
        public AsyncEventHandler(Func<T, IReadOnlyCollection<AsyncEventHandler>, Task> eventHandler, string id) : this(eventHandler, id, int.MaxValue, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncEventHandler{T}"/> class.
        /// </summary>
        /// <param name="eventHandler">Delegate returning asynchronous event handler.</param>
        /// <param name="id">Identifier of <see cref="AsyncEventHandler"/>.</param>
        /// <param name="priority">Determines order of execution.</param>
        public AsyncEventHandler(Func<T, IReadOnlyCollection<AsyncEventHandler>, Task> eventHandler, string id, int priority) : this(eventHandler, id, priority, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncEventHandler{T}"/> class.
        /// </summary>
        /// <param name="eventHandler">Delegate returning asynchronous event handler.</param>
        /// <param name="id">Identifier of <see cref="AsyncEventHandler"/>.</param>
        /// <param name="priority">Determines order of execution.</param>
        /// <param name="parallel">See <see cref="AsyncEventHandler.ExecuteInParallel"/> property.</param>
        public AsyncEventHandler(Func<T, IReadOnlyCollection<AsyncEventHandler>, Task> eventHandler, string id, int priority, bool parallel) : base(id, priority, parallel)
        {
            _asyncEvent = eventHandler;
        }

        /// <summary>
        /// Executes asynchronosuly encapsulated event handler.
        /// </summary>
        /// <param name="eventArgs">Event arguments.</param>
        /// <param name="invocationList">
        /// Read-only collection of other <see cref="AsyncEventHandler{T}"/> objects which subscribe to common event.
        /// </param>
        public async Task Execute(T eventArgs, IReadOnlyCollection<AsyncEventHandler> invocationList)
        {
            await _asyncEvent.Invoke(eventArgs, invocationList);
            _executed = true;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if(obj is Func<T, IReadOnlyCollection<AsyncEventHandler>, Task> function)
            {
                return function == _asyncEvent;
            }

            return base.Equals(obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 692590352;
            hashCode = hashCode * -1521134295 + _executed.GetHashCode();
            hashCode = hashCode * -1521134295 + SkipCurrentIteration.GetHashCode();
            hashCode = hashCode * -1521134295 + Executed.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + ExecuteInParallel.GetHashCode();
            hashCode = hashCode * -1521134295 + Priority.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Func<T, IReadOnlyCollection<AsyncEventHandler>, Task>>.Default.GetHashCode(_asyncEvent);
            return hashCode;
        }
    }
}
