using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Utils
{
    /// <summary>
    /// Allows for defering event subscription.
    /// </summary>
    public class LazySubscription : IDisposable
    {
        private readonly Delegate _eventHandler;
        private readonly Delegate? _unavailableHandler;
        private readonly string _eventName;

        private bool _isDisposed;
        private object? _subscriptionTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazySubscription"/> class.
        /// </summary>
        /// <param name="eventHandler">Event handler for target subscription.</param>
        /// <param name="unavailableHandler">Delegate to be executed if subscription attempt fails.</param>
        /// <param name="eventName">Name of the event which subscription is to be deferred.</param>
        public LazySubscription(Delegate eventHandler, Delegate? unavailableHandler, string eventName)
        {
            _eventHandler = eventHandler;
            _unavailableHandler = unavailableHandler;
            _eventName = eventName;
        }

        /// <summary>
        /// Attempts to perform subscription to the target event on provided object.
        /// If subscription fails the unavailable handler is called. 
        /// </summary>
        /// <param name="subscriptionTarget">Object where target event will be searched.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public void SubscribeOrExecute(object subscriptionTarget)
        {
            _ = subscriptionTarget ?? throw new ArgumentNullException(nameof(subscriptionTarget));

            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(LazySubscription));
            }

            if (_subscriptionTarget is not null)
            {
                return;
            }

            _subscriptionTarget = subscriptionTarget;
            bool isSubscribed = SubscribeViaReflection(subscriptionTarget, _eventHandler, _eventName);

            if (!isSubscribed)
            {
                _unavailableHandler?.DynamicInvoke(subscriptionTarget);
            }
        }

        /// <summary>
        /// Unsubscribe encapsulated subscription if viable.
        /// </summary>
        public void Dispose()
        {
            _isDisposed = true;
            if (_subscriptionTarget is not null)
            {
                UnsubscribeViaReflection(_subscriptionTarget, _eventHandler, _eventName);
            }
        }

        private static bool SubscribeViaReflection(object targetObject, Delegate eventHandler, string eventName)
        {
            EventInfo? eventInfo = targetObject.GetType().GetEvent(eventName);
            eventInfo?.AddEventHandler(targetObject, eventHandler);
            return eventInfo is not null;
        }

        private static void UnsubscribeViaReflection(object targetObject, Delegate eventHandler, string eventName)
        {
            EventInfo? eventInfo = targetObject.GetType().GetEvent(eventName);
            eventInfo?.RemoveEventHandler(targetObject, eventHandler);
        }
    }
}
