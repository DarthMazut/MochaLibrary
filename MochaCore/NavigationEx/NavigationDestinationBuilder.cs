using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx
{
    public class NavigationRequestBuilder : INavigationDestinationBuilder, INavigationRequestDetailsBuilder
    {
        private readonly INavigationModule _module;
        private string? _targetId;
        private string? _targetService;
        private NavigationType _navigationType = NavigationType.Push;
        private int _step;
        private bool _ignoreCached;
        private bool? _saveCurrent;
        private object? _parameter;
        private NavigationEventsOptions? _navigationEventsOptions;

        public NavigationRequestBuilder(INavigationModule module)
        {
            _module = module;
        }

        public INavigationRequestDetailsBuilder To(string targetId)
        {
            _navigationType = NavigationType.Push;
            _targetId = targetId;
            return this;
        }

        public INavigationRequestDetailsBuilder Modal(string targetId)
        {
            _navigationType = NavigationType.PushModal;
            _targetId = targetId;
            return this;
        }

        public INavigationRequestDetailsBuilder Pop()
        {
            _navigationType = NavigationType.Pop;
            return this;
        }

        public INavigationRequestDetailsBuilder Back() => Back(1);

        public INavigationRequestDetailsBuilder Back(int step)
        {
            _navigationType = NavigationType.Back;
            _step = step;
            return this;
        }

        public INavigationRequestDetailsBuilder Forward() => Forward(1);

        public INavigationRequestDetailsBuilder Forward(int step)
        {
            _navigationType = NavigationType.Forward;
            _step = step;
            return this;
        }

        public INavigationRequestDetailsBuilder IgnoreCached()
        {
            _ignoreCached = true;
            return this;
        }

        public INavigationRequestDetailsBuilder WithParameter(object parameter)
        {
            _parameter = parameter;
            return this;
        }

        public INavigationRequestDetailsBuilder SaveCurrent()
        {
            _saveCurrent = true;
            return this;
        }

        public INavigationRequestDetailsBuilder ClearCurrent()
        {
            _saveCurrent = false;
            return this;
        }

        public INavigationRequestDetailsBuilder ForService(string targetServiceId)
        {
            _targetService = targetServiceId;
            return this;
        }

        public INavigationService? ResolveService()
            => _targetService is null ? null : NavigationManager.FetchNavigationService(_targetService);

        public NavigationRequestData Build()
        {
            return _navigationType switch
            {
                NavigationType.Push => NavigationRequestData.CreatePushRequest(_targetId!, _module, _parameter, _saveCurrent, _ignoreCached, _navigationEventsOptions),
                NavigationType.Back => NavigationRequestData.CreateBackRequest(_step, _module, _parameter, _saveCurrent, _ignoreCached, _navigationEventsOptions),
                NavigationType.Forward => NavigationRequestData.CreateForwardRequest(_step, _module, _parameter, _saveCurrent, _ignoreCached, _navigationEventsOptions),
                NavigationType.PushModal => NavigationRequestData.CreateModalRequest(_targetId!, _module, _parameter, _ignoreCached, _navigationEventsOptions),
                NavigationType.Pop => NavigationRequestData.CreatePopRequest(_module, _parameter, _navigationEventsOptions),
                _ => throw new NotImplementedException(),
            };
        }
    }

    public interface INavigationDestinationBuilder
    {
        public INavigationRequestDetailsBuilder To(string targetId);

        public INavigationRequestDetailsBuilder Modal(string targetId);

        public INavigationRequestDetailsBuilder Pop();

        public INavigationRequestDetailsBuilder Back();

        public INavigationRequestDetailsBuilder Back(int step);

        public INavigationRequestDetailsBuilder Forward();

        public INavigationRequestDetailsBuilder Forward(int step);
    }

    public interface INavigationRequestDetailsBuilder
    {
        public INavigationRequestDetailsBuilder WithParameter(object parameter);

        public INavigationRequestDetailsBuilder IgnoreCached();

        public INavigationRequestDetailsBuilder SaveCurrent();

        public INavigationRequestDetailsBuilder ClearCurrent();

        public INavigationRequestDetailsBuilder ForService(string targetServiceId);
    }
}
