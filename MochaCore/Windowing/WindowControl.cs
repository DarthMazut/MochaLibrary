using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Prvoides implementation of <see cref="IWindowControl"/>.
    /// </summary>
    public class WindowControl : IWindowControl, IWindowControlInitialize
    {
        protected IWindowModule? _module;
        private bool _isInitialized = false;

        /// <inheritdoc/>
        public event EventHandler? Opened;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <inheritdoc/>
        public object View
        {
            get
            {
                InitializationGuard();
                return _module!.View;
            }
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

        /// <inheritdoc/>
        void IWindowControlInitialize.Initialize(IWindowModule module)
        {
            _module = module;

            InitializeCore();

            _isInitialized = true;
        }

        /// <summary>
        /// Contains core logic for initialization.
        /// </summary>
        protected virtual void InitializeCore()
        {
            _module!.Opened += ModuleOpened;
            _module!.Closed += ModuleClosed;
            _module!.Disposed += ModuleDisposed;
        }

        /// <summary>
        /// Contains core logic of uninitialization.
        /// </summary>
        protected virtual void UninitializeCore()
        {
            _module!.Opened -= ModuleOpened;
            _module!.Closed -= ModuleClosed;
            _module!.Disposed -= ModuleDisposed;
        }

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> if current object isn't initialized.
        /// </summary>
        protected void InitializationGuard()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException($"{GetType().Name} was not initialized.");
            }
        }

        private void ModuleOpened(object? sender, EventArgs e)
        {
            Opened?.Invoke(this, EventArgs.Empty);
        }

        private void ModuleClosed(object? sender, EventArgs e)
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void ModuleDisposed(object? sender, EventArgs e)
        {
            Disposed?.Invoke(this, EventArgs.Empty);
            if (_isInitialized)
            {
                Uninitialize();
            }
        }

        private void Uninitialize()
        {
            UninitializeCore();

            _module = null;
            _isInitialized = false;
        }
    }

    /// <summary>
    /// Provides API form managing related window.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
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
