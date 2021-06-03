using System;
using System.Collections.Generic;
using System.Text;

namespace Mocha.Events
{
    /// <summary>
    /// Allows for handling technology-specific events within technology-independent modules.
    /// </summary>
    public static class AppEventManager
    {
        private static IEventProvider _eventProvider;

        /// <summary>
        /// Returns provided implementation of <see cref="IEventProvider"/>.
        /// Throws if no such was provided.
        /// </summary>
        public static IEventProvider EventProvider
        {
            get
            {
                if (_eventProvider != null)
                {
                    return _eventProvider;
                }
                else
                {
                    throw new InvalidOperationException($"No implementation of {typeof(IEventProvider)} was provided. Use *Initialize* method first.");
                }
            }
        }

        /// <summary>
        /// Initializes <see cref="AppEventManager"/> class. <see cref="IEventProvider"/>
        /// instance is required.
        /// </summary>
        /// <param name="eventProvider">Technology-specific implementation of <see cref="IEventProvider"/>.</param>
        public static void Initialize(IEventProvider eventProvider)
        {
            _eventProvider = eventProvider;
        }
    }
}
