using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Prvoides implementation of <see cref="IWindowControl"/>.
    /// </summary>
    public class WindowControl : BaseWindowControl, IWindowControl
    {
        protected IWindowModule? _customModule;

        /// <inheritdoc/>
        public ModuleWindowState WindowState
        {
            get
            {
                InitializationGuard();
                return _customModule!.WindowState;
            }
        }

        /// <inheritdoc/>
        public new IWindowModule Module => (IWindowModule)base.Module;

        /// <inheritdoc/>
        public event EventHandler<CancelEventArgs>? Closing;

        /// <inheritdoc/>
        public event EventHandler<WindowStateChangedEventArgs>? StateChanged;

        /// <inheritdoc/>
        public void Maximize()
        {
            InitializationGuard();
            _customModule!.Maximize();
        }

        /// <inheritdoc/>
        public void Minimize()
        {
            InitializationGuard();
            _customModule!.Minimize();
        }

        /// <inheritdoc/>
        public void Restore()
        {
            InitializationGuard();
            _customModule!.Restore();
        }

        /// <inheritdoc/>
        public void Hide()
        {
            InitializationGuard();
            _customModule!.Hide();
        }

        /// <inheritdoc/>
        protected override void InitializeCore()
        {
            if (_module is IWindowModule customModule)
            {
                _customModule = customModule;
                _customModule.Closing += ModuleClosing;
                _customModule.StateChanged += ModuleStateChanged;
            }
            else
            {
                throw new InvalidCastException($"{GetType().Name} can only be initialized with {typeof(IWindowModule)}.");
            }

            base.InitializeCore();
        }

        /// <inheritdoc/>
        protected override void UninitializeCore()
        {
            _customModule!.Closing -= ModuleClosing;
        }

        private void ModuleClosing(object? sender, CancelEventArgs e)
        {
            CancelEventArgs args = new();
            Closing?.Invoke(this, args);
            e.Cancel = args.Cancel;
        }

        private void ModuleStateChanged(object? sender, WindowStateChangedEventArgs e)
        {
            StateChanged?.Invoke(this, e);
        }
    }

    /// <summary>
    /// Prvoides implementation of <see cref="IWindowControl"/>.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public class WindowControl<T> : WindowControl, IWindowControl<T> where T : class, new()
    {
        private Action<T>? _customizeDelegate;

        /// <inheritdoc/>
        public T Properties
        {
            get
            {
                InitializationGuard();
                return Module.Properties;
            }
        }

        /// <inheritdoc/>
        new public IWindowModule<T> Module => (IWindowModule<T>)base.Module;

        /// <inheritdoc/>
        public void Customize(Action<T> customizeDelegate) => _customizeDelegate = customizeDelegate;

        /// <inheritdoc/>
        protected override void InitializeCore()
        {
            if (_customModule is IWindowModule<T> typedModule)
            {
                base.InitializeCore();
                _customizeDelegate?.Invoke(typedModule.Properties);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
