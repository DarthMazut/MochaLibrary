using System;
using System.Collections.Generic;

namespace MochaCore.Events
{
    /// <summary>
    /// Allows for handling technology-specific events within technology-independent modules.
    /// </summary>
    public static class AppEventManager
    {
        private static readonly Dictionary<string, object> _events = new();

        /// <summary>
        /// Registers provided implementation of <see cref="IEventProvider{TEventArgs}"/> with given ID.
        /// </summary>
        /// <typeparam name="TEventArgs">Type of event arguments.</typeparam>
        /// <param name="id"><see cref="IEventProvider{TEventArgs}"/> unique identifier.</param>
        /// <param name="eventProvider">Implementation of <see cref="IEventProvider{TEventArgs}"/>.</param>
        public static void IncludeEventProvider<TEventArgs>(string id, IEventProvider<TEventArgs> eventProvider) where TEventArgs : BaseEventArgs
        {
            if (_events.ContainsKey(id))
            {
                throw new InvalidOperationException($"Event provider with id {id} was already included.");
            }
            else
            {
                _events.Add(id, eventProvider);
            }
        }

        /// <summary>
        /// Returns provided implementation of <see cref="IEventProvider{TEventArgs}"/> corresponding to given ID.
        /// </summary>
        /// <typeparam name="TEventArgs">Type of event arguments.</typeparam>
        /// <param name="id">ID of requesting <see cref="IEventProvider{TEventArgs}"/>.</param>
        public static IEventProvider<TEventArgs> RequestEventProvider<TEventArgs>(string id) where TEventArgs : BaseEventArgs
        {
            if (_events.ContainsKey(id))
            {
                if (_events[id] is IEventProvider<TEventArgs> eventHandler)
                {
                    return eventHandler;
                }
                else
                {
                    throw new InvalidOperationException($"Requested type {typeof(TEventArgs)} did not match the one registered with id {id}.");
                }
            }

            throw new InvalidOperationException($"Requested event provider {id} was never registered.");
        }
    }
}
