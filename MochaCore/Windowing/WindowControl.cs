using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Provides API form managing related window.
    /// </summary>
    public class WindowControl : IWindowControl
    {
        protected IWindowModule? _module;
        private bool _isInitialized = false;

        /// <inheritdoc/>
        public event EventHandler? Opened;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public object View
        {
            get
            {
                InitializationGuard();
                return _module!.View;
            }
        }

        public virtual void Initialize(IWindowModule module)
        {
            _module = module;
            _isInitialized = true;
        }

        public void Uninitialize()
        {
            _module = null;
            _isInitialized = false;
        }

        /// <inheritdoc/>
        public void Close()
        {
            InitializationGuard();

            _module!.Close();
        }

        /// <inheritdoc/>
        public void Close(object? result)
        {
            InitializationGuard();
            _module!.Close(result);
        }

        protected void InitializationGuard()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException($"{GetType().Name} was not initialized.");
            }
        }
    }

    /// <summary>
    /// Provides API form managing related window.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WindowControl<T> : WindowControl, IWindowControl<T> where T : class, new()
    {
        /// <inheritdoc/>
        public T Properties
        {
            get
            {
                InitializationGuard();
                return ((IWindowModule<T>)_module!).Properties;
            }
        }
    }
}
