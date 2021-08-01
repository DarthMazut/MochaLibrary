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
        /// Executes given delegate on Main Thread and blocks until execution in finished.
        /// </summary>
        /// <param name="action">Delegate to be executed on Main Thread</param>
        void RunOnMainThread(Action action);

        /// <summary>
        /// Executes asynchronous delegate on Main Thread and blocks until execution is finished.
        /// </summary>
        /// <param name="func">Asynchronous function to be executed on Main Thread.</param>
        Task RunOnMainThreadAsync(Func<Task> func);

        /// <summary>
        /// Enqueues given delegate to be executed by Main Thread when possible.
        /// </summary>
        /// <param name="action">Delegate to be enqueued</param>
        void EnqueueOnMainThread(Action action);

        /// <summary>
        /// Returns control to the Main/UI thread and executes succeeding code 
        /// when Main/UI thread is free.
        /// </summary>
        Task Yield();
    }
}
