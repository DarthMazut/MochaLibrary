using System;
using System.Threading.Tasks;

namespace MochaCore.Dispatching
{
    /// <summary>
    /// Allows for code execution on Main/UI Thread.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Checks whether the calling thread is the Main Thread.
        /// </summary>
        public bool HasThreadAccess { get; }

        /// <summary>
        /// Executes given delegate on Main Thread and blocks until execution in finished.
        /// </summary>
        /// <param name="action">Delegate to be executed on Main Thread.</param>
        void RunOnMainThread(Action action);

        /// <summary>
        /// Executes given delegate on Main Thread and blocks until <see cref="Task"/>
        /// associated with provided delegate is finished.
        /// </summary>
        /// <param name="asyncAction">Asynchronous delegate to be executed on Main Thread.</param>
        void RunOnMainThread(Func<Task> asyncAction);

        /// <summary>
        /// Executes delegate on Main Thread and returns <see cref="Task"/>
        /// which finishes when delegate is executed.
        /// </summary>
        /// <param name="action">Delegate to be executed on Main Thread.</param>
        Task RunOnMainThreadAsync(Action action);

        /// <summary>
        /// Executes asynchronous delegate on Main Thread and returns <see cref="Task"/>
        /// which finishes when delegate is executed.
        /// </summary>
        /// <param name="func">Asynchronous delegate to be executed on Main Thread.</param>
        Task RunOnMainThreadAsync(Func<Task> func);

        /// <summary>
        /// Enqueues given delegate to be executed on Main Thread when possible.
        /// </summary>
        /// <param name="action">Delegate to be enqueued on Main Thread.</param>
        void EnqueueOnMainThread(Action action);

        /// <summary>
        /// Enqueues given delegate to be executed on Main Thread when possible.
        /// </summary>
        /// <param name="asyncAction">Asynchronous delegate to be enqueued on Main Thread.</param>
        void EnqueueOnMainThread(Func<Task> asyncAction);

        /// <summary>
        /// Returns control to the Main/UI thread and executes succeeding code 
        /// when Main/UI thread is free.
        /// </summary>
        Task Yield();
    }
}
