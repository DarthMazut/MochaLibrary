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
    public class LazySubscruption : IDisposable
    {
        private readonly Delegate _eventHandler;
        private readonly Delegate? _unavailableHandler;
        private readonly string _eventName;

        private bool _isDisposed;
        private object? _subscriptionTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazySubscruption"/> class.
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <param name="unavailableHandler"></param>
        /// <param name="eventName"></param>
        public LazySubscruption(Delegate eventHandler, Delegate? unavailableHandler, string eventName)
        {
            _eventHandler = eventHandler;
            _unavailableHandler = unavailableHandler;
            _eventName = eventName;
        }

        public void SubscribeOrExecute(object subscriptionTarget)
        {
            _ = subscriptionTarget ?? throw new ArgumentNullException(nameof(subscriptionTarget));

            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(LazySubscruption));
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
            if (eventInfo is null)
            {
                return false;
            }

            eventInfo.AddEventHandler(targetObject, eventHandler);
            return true;
        }

        private static void UnsubscribeViaReflection(object targetObject, Delegate eventHandler, string eventName)
        {
            EventInfo? eventInfo = targetObject.GetType().GetEvent(eventName);
            if (eventInfo is null)
            {
                return;
            }

            eventInfo.RemoveEventHandler(targetObject, eventHandler);
        }
    }
}
