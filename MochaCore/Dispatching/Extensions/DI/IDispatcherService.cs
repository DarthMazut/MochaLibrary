﻿using System;

namespace MochaCore.Dispatching.Extensions.DI
{
    /// <summary>
    /// Allows for retrieving main thread dispatcher object for Dependency Injection architecure.
    /// </summary>
    public interface IDispatcherService
    {
        /// <summary>
        /// Returns registered <see cref="IDispatcher"/> implementation.
        /// Throws if dispatcher hasn't been registered.
        /// </summary>
        /// <exception cref="NullReferenceException"/>
        IDispatcher GetMainThreadDispatcher();
    }
}
