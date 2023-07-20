using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx.Extensions
{
    /// <summary>
    /// Provides base class for technology specific implementations of <see cref="INavigationModule"/>.
    /// </summary>
    /// <typeparam name="TView">Type of technology-specific view object.</typeparam>
    /// <typeparam name="TDataContext">Type of object which serves as data context for view object.</typeparam>
    public abstract class BaseNavigationModule<TView, TDataContext> : INavigationLifecycleModule
        where TView : class
        where TDataContext : class, INavigatable
    {
        private readonly Func<TView> _viewBuilder;
        private readonly Func<TDataContext>? _dataContextBuilder;

        private bool _isInitialized = false;
        private TView? _view;
        private TDataContext? _dataContext;

        protected BaseNavigationModule(
            string id,
            Func<TView> viewBuilder,
            Func<TDataContext>? dataContextBuilder,
            NavigationModuleLifecycleOptions? lifecycleOptions)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id)); ;
            _viewBuilder = viewBuilder ?? throw new ArgumentNullException(nameof(viewBuilder));
            _dataContextBuilder = dataContextBuilder;
            LifecycleOptions = lifecycleOptions ?? new NavigationModuleLifecycleOptions();
        }

        /// <inheritdoc/>
        public string Id { get; }

        /// <inheritdoc/>
        public object? View => _view;

        /// <inheritdoc/>
        public Type ViewType => typeof(TView);

        /// <inheritdoc/>
        public INavigatable? DataContext => _dataContext;

        /// <inheritdoc/>
        public Type? DataContextType => typeof(TDataContext);

        /// <inheritdoc/>
        public bool IsInitialized => _isInitialized;

        /// <inheritdoc/>
        public NavigationModuleLifecycleOptions LifecycleOptions { get; }

        /// <summary>
        /// Retrieves <i>DataContext</i> from provided object.
        /// Throws if retrieved object is different than <see langword="null"/>
        /// or expected <see cref="INavigatable"/> implementation.
        /// </summary>
        /// <param name="view">The view from which the data context is retrieved.</param>
        protected abstract TDataContext? GetDataContext(TView view);

        /// <summary>
        /// Sets the DataContext on provided view object.
        /// </summary>
        /// <param name="view">Object which data context is to be set.</param>
        /// <param name="dataContext">Data context to be set.</param>
        protected abstract void SetDataContext(TView view, TDataContext? dataContext);

        /// <inheritdoc/>
        public void Initialize()
        {
            InitializationGuard();

            _view = _viewBuilder.Invoke();
            _dataContext = _dataContextBuilder?.Invoke() ?? GetDataContext(_view) ?? Activator.CreateInstance<TDataContext>();

            if (GetDataContext(_view) is null)
            {
                SetDataContext(_view, _dataContext);
            }

            _isInitialized = true;
        }

        /// <inheritdoc/>
        public void Uninitialize()
        {
            (_dataContext?.Navigator as IDisposable)?.Dispose();

            if (_view is not null)
            {
                SetDataContext(_view, null);
            }

            _view = null;

            if (LifecycleOptions.DisposeDataContextOnUninitialize && _dataContext is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _dataContext = null;
            _isInitialized = false;
        }

        private void InitializationGuard()
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException($"{nameof(Initialize)} has been called on already initialized {nameof(INavigationLifecycleModule)} instance.");
            }
        }
    }
}
