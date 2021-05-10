using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Dispatching
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
        /// Enqueues given delegate to be executed by Main Thread when possible.
        /// </summary>
        /// <param name="action">Delegate to be enqueued</param>
        void EnqueueOnMainThread(Action action);
    }
}
