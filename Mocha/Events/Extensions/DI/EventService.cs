using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Events.Extensions.DI
{
    /// <summary>
    /// Allows for handling technology-specific events within technology-independent modules.
    /// Use it for Dependency Injection architecture.
    /// </summary>
    public class EventService : IEventService
    {
        /// <inheritdoc/>
        public IEventProvider EventProvider => AppEventManager.EventProvider;
    }
}
