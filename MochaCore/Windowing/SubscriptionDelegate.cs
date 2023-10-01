using System;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Encapsulates special <c>SubscribeOrExecute</c> and <c>Unsubscribe</c> delegates.
    /// These are used by <see cref="WindowControl"/> implementations, to queue
    /// operations to be executed during <see cref="WindowControl"/> initialization.
    /// </summary>
    public class SubscriptionDelegate : IDisposable
    {
        private bool _isDisposed;
        private IBaseWindowModule? _module;
        private Action<IBaseWindowModule> _subscribeOrExecute;
        private Action<IBaseWindowModule> _unsubscribe;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionDelegate"/> class.
        /// </summary>
        /// <param name="subscribeOrExecute">Subscription delegate.</param>
        /// <param name="unsubscribe">Subscription removal delegate.</param>
        public SubscriptionDelegate(
            Action<IBaseWindowModule> subscribeOrExecute,
            Action<IBaseWindowModule> unsubscribe)
        {
            _subscribeOrExecute = subscribeOrExecute;
            _unsubscribe = unsubscribe;
        }

        /// <summary>
        /// Subscribes to event specified within provided delegate or executes
        /// feature unavailable delegate if no subscription was possible.
        /// </summary>
        /// <param name="module">Related module.</param>
        public void SubscribeOrExecute(IBaseWindowModule module)
        {
            if (!_isDisposed)
            {
                _module = module;
                _subscribeOrExecute.Invoke(module);
            }
        }

        /// <summary>
        /// Removes subscription made during <see cref="SubscribeOrExecute(IBaseWindowModule)"/> call, if any.
        /// </summary>
        public void Dispose()
        {
            if (_module is not null)
            {
                _unsubscribe.Invoke(_module);
            }
            _isDisposed = true;
        }
    }
}