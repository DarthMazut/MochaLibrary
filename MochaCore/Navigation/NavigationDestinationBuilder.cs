using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Navigation
{
    /// <summary>
    /// Provides implementation of <see cref="INavigationDestinationBuilder"/> and <see cref="INavigationRequestDetailsBuilder"/>
    /// interfaces.
    /// </summary>
    public class NavigationRequestBuilder : INavigationDestinationBuilder, INavigationRequestDetailsBuilder
    {
        private readonly object? _owner;
        private string? _targetId;
        private NavigationType _navigationType = NavigationType.Push;
        private int _step;
        private bool _ignoreCached;
        private bool? _saveCurrent;
        private object? _parameter;
        private NavigationEventsOptions? _navigationEventsOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationRequestBuilder"/> class.
        /// </summary>
        /// <param name="owner"></param>
        public NavigationRequestBuilder(object? owner)
        {
            _owner = owner;
        }

        /// <inheritdoc/>
        public INavigationRequestDetailsBuilder To(string targetId)
        {
            _navigationType = NavigationType.Push;
            _targetId = targetId;
            return this;
        }

        /// <inheritdoc/>
        public INavigationRequestDetailsBuilder Modal(string targetId)
        {
            _navigationType = NavigationType.PushModal;
            _targetId = targetId;
            return this;
        }

        /// <inheritdoc/>
        public INavigationRequestDetailsBuilder Pop() => Pop(null);

        /// <inheritdoc/>
        public INavigationRequestDetailsBuilder Pop(object? returnData)
        {
            _navigationType = NavigationType.Pop;
            _parameter = returnData;
            return this;
        }

        /// <inheritdoc/>
        public INavigationRequestDetailsBuilder Back() => Back(1);

        /// <inheritdoc/>
        public INavigationRequestDetailsBuilder Back(int step)
        {
            _navigationType = NavigationType.Back;
            _step = step;
            return this;
        }

        /// <inheritdoc/>
        public INavigationRequestDetailsBuilder Forward() => Forward(1);

        /// <inheritdoc/>
        public INavigationRequestDetailsBuilder Forward(int step)
        {
            _navigationType = NavigationType.Forward;
            _step = step;
            return this;
        }

        /// <inheritdoc/>
        public INavigationRequestDetailsBuilder IgnoreCached()
        {
            _ignoreCached = true;
            return this;
        }

        /// <inheritdoc/>
        public INavigationRequestDetailsBuilder WithParameter(object parameter)
        {
            _parameter = parameter;
            return this;
        }

        /// <inheritdoc/>
        public INavigationRequestDetailsBuilder SaveCurrent()
        {
            _saveCurrent = true;
            return this;
        }

        /// <inheritdoc/>
        public INavigationRequestDetailsBuilder ClearCurrent()
        {
            _saveCurrent = false;
            return this;
        }

        /// <summary>
        /// Creates the <see cref="NavigationRequestData"/>.
        /// </summary>
        public NavigationRequestData Build()
        {
            return _navigationType switch
            {
                NavigationType.Push => NavigationRequestData.CreatePushRequest(_targetId!, _owner, _parameter, _saveCurrent, _ignoreCached, _navigationEventsOptions),
                NavigationType.Back => NavigationRequestData.CreateBackRequest(_step, _owner, _parameter, _saveCurrent, _ignoreCached, _navigationEventsOptions),
                NavigationType.Forward => NavigationRequestData.CreateForwardRequest(_step, _owner, _parameter, _saveCurrent, _ignoreCached, _navigationEventsOptions),
                NavigationType.PushModal => NavigationRequestData.CreateModalRequest(_targetId!, _owner, _parameter, _ignoreCached, _navigationEventsOptions),
                NavigationType.Pop => NavigationRequestData.CreatePopRequest(_owner, _parameter, _navigationEventsOptions),
                _ => throw new NotImplementedException(),
            };
        }
    }

    /// <summary>
    /// Provides methods to specify destination and type of navigation process while creating 
    /// <see cref="NavigationRequestData"/> object.
    /// </summary>
    public interface INavigationDestinationBuilder
    {
        /// <summary>
        /// Specifies destination of creating <see cref="NavigationType.Push"/> navigation request.
        /// </summary>
        /// <param name="targetId">Identifier of the target <see cref="INavigationModule"/>.</param>
        public INavigationRequestDetailsBuilder To(string targetId);

        /// <summary>
        /// Specifies destination of creating <see cref="NavigationType.PushModal"/> navigation request.
        /// </summary>
        /// <param name="targetId">Identifier of the target <see cref="INavigationModule"/>.</param>
        public INavigationRequestDetailsBuilder Modal(string targetId);

        /// <summary>
        /// Specifies <see cref="NavigationType.Pop"/> navigation type for creating request.
        /// </summary>
        public INavigationRequestDetailsBuilder Pop();

        /// <summary>
        /// Specifies return data of creating <see cref="NavigationType.Pop"/> navigation request.
        /// </summary>
        /// <param name="returnData">The data to be returned from the modal navigation.</param>
        public INavigationRequestDetailsBuilder Pop(object? returnData);

        /// <summary>
        /// Specifies <see cref="NavigationType.Back"/> navigation type for creating request.
        /// </summary>
        public INavigationRequestDetailsBuilder Back();

        /// <summary>
        /// Specifies the number of items to be traversed back on the navigation stack of
        /// creating <see cref="NavigationType.Back"/> navigation request.
        /// </summary>
        /// <param name="step">
        /// Determines by how many elements the pointer to the current navigation element will be shifted back.
        /// </param>
        public INavigationRequestDetailsBuilder Back(int step);

        /// <summary>
        /// Specifies <see cref="NavigationType.Forward"/> navigation type for creating request.
        /// </summary>
        public INavigationRequestDetailsBuilder Forward();

        /// <summary>
        /// Specifies the number of items to be traversed forward on the navigation stack of
        /// creating <see cref="NavigationType.Forward"/> navigation request.
        /// </summary>
        /// <param name="step">
        /// Determines by how many elements the pointer to the current navigation element will be shifted forward.
        /// </param>
        public INavigationRequestDetailsBuilder Forward(int step);
    }

    /// <summary>
    /// Provides methods to specify details of navigation process while creating 
    /// <see cref="NavigationRequestData"/> object.
    /// </summary>
    public interface INavigationRequestDetailsBuilder
    {
        /// <summary>
        /// Allows to add parameter to creating request.
        /// </summary>
        /// <param name="parameter">Parameter to be add.</param>
        public INavigationRequestDetailsBuilder WithParameter(object parameter);

        /// <summary>
        /// Allows to specify that cached <see cref="INavigationModule"/> object
        /// will be ignored (if any) while executing creating request.
        /// </summary>
        public INavigationRequestDetailsBuilder IgnoreCached();

        /// <summary>
        /// Allows to specify that current <see cref="INavigationModule"/> object
        /// will be cached while executing creating request.
        /// </summary>
        public INavigationRequestDetailsBuilder SaveCurrent();

        /// <summary>
        /// Allows to explicitly specify that current <see cref="INavigationModule"/> object
        /// won't be cached while executing creating request.
        /// </summary>
        public INavigationRequestDetailsBuilder ClearCurrent();
    }
}
