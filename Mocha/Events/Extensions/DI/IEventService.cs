using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Events.Extensions.DI
{
    /// <summary>
    /// Allows for handling technology-specific events within technology-independent modules.
    /// Use it for Dependency Injection architecture.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Returns provided implementation of <see cref="IEventProvider{TEventArgs}"/> corresponding to given ID.
        /// </summary>
        /// <typeparam name="TEventArgs">Type of event arguments.</typeparam>
        /// <param name="id">ID of requesting <see cref="IEventProvider{TEventArgs}"/>.</param>
        IEventProvider<TEventArgs> RequestEventProvider<TEventArgs>(string id) where TEventArgs : EventArgs;
    }
}
