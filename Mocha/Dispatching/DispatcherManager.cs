using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Dispatching
{
    /// <summary>
    /// Allows for registering and retrieving <see cref="IDispatcher"/> object
    /// which allows for code execution on the application Main Thread in MVVM architecure.
    /// </summary>
    public static class DispatcherManager
    {
        private static IDispatcher _dispatcher;

        /// <summary>
        /// Sets an <see cref="IDispatcher"/> implementation which allows 
        /// for code execution on the application Main Thread
        /// </summary>
        /// <param name="dispatcher">Object implementing <see cref="IDispatcher"/> interface.</param>
        public static void SetMainThreadDispatcher(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Returns registered <see cref="IDispatcher"/> implementation.
        /// Throws if dispatcher hasn't been registered.
        /// </summary>
        /// <exception cref="NullReferenceException"/>
        public static IDispatcher GetMainThreadDispatcher()
        {
            if(_dispatcher == null)
            {
                throw new NullReferenceException("The dispatcher object was requested but it was never registered.");
            }

            return _dispatcher;
        }

    }
}
