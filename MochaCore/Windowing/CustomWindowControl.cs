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
    public class CustomWindowControl : WindowControl, ICustomWindowControl, IMaximizeWindow, IMinimizeWindow, IClosingWindow
    {
        protected ICustomWindowModule? _customModule;

        /// <inheritdoc/>
        public event EventHandler<CancelEventArgs>? Closing;

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
        protected override void InitializeCore()
        {
            if (_module is ICustomWindowModule customModule)
            {
                _customModule = customModule;
                _customModule.Closing += ModuleClosing;
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
