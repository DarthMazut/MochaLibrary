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
        public event EventHandler<CancelEventArgs>? Closing
        {
            add
            {
                InitializationGuard();
                _customModule!.Closing += value;
            }
            remove
            {
                InitializationGuard();
                _customModule!.Closing -= value;
            }
        }


        /// <inheritdoc/>
        public void Maximize()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Minimize()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void Initialize(IWindowModule module)
        {
            if (module is ICustomWindowModule customModule)
            {
                _customModule = customModule;
            }
            else
            {
                throw new InvalidCastException($"{GetType().Name} can only be initialized with {typeof(ICustomWindowModule)}.");
            }
            base.Initialize(module);
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
