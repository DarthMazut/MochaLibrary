using System;

namespace MochaCore.Windowing
{
    public class SubscriptionDelegate : IDisposable
    {
        private bool _isDisposed;
        private IBaseWindowModule _module;
        private Action<IBaseWindowModule> _subscribeOrExecute;
        private Action<IBaseWindowModule> _unsubscribe;

        public SubscriptionDelegate(
            Action<IBaseWindowModule> subscribeOrExecute,
            Action<IBaseWindowModule> unsubscribe)
        {
            _subscribeOrExecute = subscribeOrExecute;
            _unsubscribe = unsubscribe;
        }

        public void SubscribeOrExecute(IBaseWindowModule module)
        {
            if (!_isDisposed)
            {
                _module = module;
                _subscribeOrExecute.Invoke(module);
            }
        }

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