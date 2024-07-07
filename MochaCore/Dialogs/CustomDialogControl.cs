using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomDialogControl : DataContextDialogControl, ICustomDialogControl, IDialogControlInitialize
    {
        /// <inheritdoc/>
        public new ICustomDialogModule Module
        {
            get
            {
                InitializationGuard();
                return (ICustomDialogModule)_module!;
            }
        }

        /// <inheritdoc/>
        public event EventHandler? Opened;

        /// <inheritdoc/>
        public event EventHandler<CancelEventArgs>? Closing;

        /// <inheritdoc/>
        public void Close(bool? result) => Module.Close(result);

        /// <inheritdoc/>
        void IDialogControlInitialize.Initialize(IDataContextDialogModule module)
        {
            if (module is ICustomDialogModule typedModule)
            {
                _module = typedModule;
                InitializeCore(module);
            }
            else
            {
                throw new InvalidOperationException($"{typeof(CustomDialogControl)} cannot be initialized with module which is not {nameof(ICustomDialogModule)}");
            }
        }

        /// <inheritdoc/>
        protected override void InitializeOverride()
        {
            base.InitializeOverride();
            ((ICustomDialogModule)_module!).Opened += ModuleOpened;
            ((ICustomDialogModule)_module!).Closing += ModuleClosing;
        }

        /// <inheritdoc/>
        protected override void UninitializeOverride()
        {
            base.UninitializeOverride();
            Module!.Opened -= ModuleOpened;
            Module!.Closing -= ModuleClosing;
        }

        private void ModuleClosing(object? sender, CancelEventArgs e) => Closing?.Invoke(this, e);

        private void ModuleOpened(object? sender, EventArgs e) => Opened?.Invoke(this, e);
    }

    public class CustomDialogControl<T> : CustomDialogControl, ICustomDialogControl<T>, IDialogControlInitialize where T : new()
    {
        private readonly List<Action<T>> _customizeDelegates = new();

        /// <inheritdoc/>
        public new ICustomDialogModule<T> Module
        {
            get
            {
                InitializationGuard();
                return (ICustomDialogModule<T>)_module!;
            }
        }

        /// <inheritdoc/>
        public T Properties => Module.Properties;

        /// <inheritdoc/>
        public void Customize(Action<T> customizeDelegate) => _customizeDelegates.Add(customizeDelegate);

        /// <inheritdoc/>
        void IDialogControlInitialize.Initialize(IDataContextDialogModule module)
        {
            if (module is ICustomDialogModule<T> typedModule)
            {
                _module = typedModule;
                InitializeCore(module);
                _customizeDelegates.ForEach(d => d.Invoke(Properties));
            }
            else
            {
                throw new InvalidOperationException($"{typeof(CustomDialogControl<T>)} cannot be initialized with module which is not {nameof(ICustomDialogModule<T>)}");
            }
        }
    }
}
