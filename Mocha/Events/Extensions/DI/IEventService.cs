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
        /// Returns provided implementation of <see cref="IEventProvider"/>.
        /// Throws if no such was provided.
        /// </summary>
        IEventProvider EventProvider { get; }
    }
}
