using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Dispatching.Extensions.DI
{
    /// <summary>
    /// Provides implementation for <see cref="IDispatcherService"/>.
    /// </summary>
    public class DispatcherService : IDispatcherService
    {
        /// <inheritdoc/>
        public IDispatcher GetMainThreadDispatcher()
        {
            return DispatcherManager.GetMainThreadDispatcher();
        }
    }
}
