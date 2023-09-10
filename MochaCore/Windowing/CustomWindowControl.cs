using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Prvoides implementation of <see cref="ICustomWindowControl"/>.
    /// </summary>
    public class CustomWindowControl : WindowControl, ICustomWindowControl
    {
        protected ICustomWindowModule? _customModule;

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
            if (_module is ICustomWindowModule customModule)
            {
                _customModule = customModule;
                _customModule.Closing += ModuleClosing;
                _customModule.StateChanged += ModuleStateChanged;
            }
            else
            {
                throw new InvalidCastException($"{GetType().Name} can only be initialized with {typeof(ICustomWindowModule)}.");
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
    /// Prvoides implementation of <see cref="ICustomWindowControl"/>.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public class CustomWindowControl<T> : CustomWindowControl, ICustomWindowControl<T> where T : class, new()
    {
        /// <inheritdoc/>
        public T Properties
        {
            get
            {
                InitializationGuard();
                return ((ICustomWindowModule<T>)_module!).Properties;
            }
        }
    }
}
