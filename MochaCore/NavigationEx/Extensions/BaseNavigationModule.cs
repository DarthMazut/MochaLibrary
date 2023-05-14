using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.NavigationEx.Extensions
{
    public abstract class BaseNavigationModule : INavigationLifecycleModule
    {
        private readonly Func<object> _viewBuilder;
        private readonly Func<INavigatable>? _dataContextBuilder;
        private bool _isInitialized = false;
        private object? _view;
        private INavigatable? _dataContext;

        public BaseNavigationModule(
            string id,
            Func<object> viewBuilder,
            Type viewType,
            Func<INavigatable>? dataContextBuilder,
            Type dataContextType,
            NavigationModuleLifecycleOptions lifecycleOptions)
        {
            Id = id;
            ViewType = viewType;
            DataContextType = dataContextType;
            _viewBuilder = viewBuilder;
            _dataContextBuilder = dataContextBuilder;
            LifecycleOptions = lifecycleOptions;
        }

        public BaseNavigationModule(object view, INavigatable dataContext)
        {
            Id = INavigationModule.RootId;
            _isInitialized = true;
            _viewBuilder = () => view;
            _view = view;
            ViewType = view.GetType();
            _dataContextBuilder = () => dataContext;
            _dataContext = dataContext;
            DataContextType = dataContext.GetType();
            LifecycleOptions = new NavigationModuleLifecycleOptions
            {
                PreferCache = true,
                DisposeDataContextOnUninitialize = false
            };

        }

        /// <inheritdoc/>
        public event EventHandler? Initialized;

        /// <inheritdoc/>
        public event EventHandler? Uninitialized;

        public string Id { get; }

        public Type ViewType { get; }

        public Type DataContextType { get; }

        public object? View => _view;

        public INavigatable? DataContext => _dataContext;

        public bool IsInitialized => _isInitialized;

        public NavigationModuleLifecycleOptions LifecycleOptions { get; }

        public abstract INavigatable? GetDataContext(object view);

        public abstract void SetDataContext(object view, INavigatable? dataContext);

        public void Initialize()
        {
            InitializationGuard();

            _view = _viewBuilder.Invoke();
            _dataContext = _dataContextBuilder?.Invoke() ?? GetDataContext(_view);
            if (_dataContextBuilder is not null)
            {
                SetDataContext(_view, _dataContext);
            }

            _isInitialized = true;
            Initialized?.Invoke(this, EventArgs.Empty);
        }

        public void Initialize(object view)
        {
            InitializationGuard();

            _view = view;
            _dataContext = _dataContextBuilder?.Invoke() ?? GetDataContext(_view);

            if (_dataContextBuilder is not null)
            {
                SetDataContext(_view, _dataContext);
            }

            _isInitialized = true;
            Initialized?.Invoke(this, EventArgs.Empty);
        }

        public void Initialize(object view, INavigatable dataContext)
        {
            InitializationGuard();

            _view = view;
            _dataContext = dataContext;
            SetDataContext(view, dataContext);

            _isInitialized = true;
            Initialized?.Invoke(this, EventArgs.Empty);
        }

        public void Uninitialize()
        {
            _dataContext?.Navigator.Dispose();

            if (_view is not null)
            {
                SetDataContext(_view, null);
            }

            _view = null;
            _dataContext = null;
            _isInitialized = false;
            Uninitialized?.Invoke(this, EventArgs.Empty);
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
