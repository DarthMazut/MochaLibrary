using System;

namespace MochaCore.DialogsEx
{
    /// <summary> 
    /// Marks implementing class as dialog logic (DataContext) for 
    /// <see cref="ICustomDialogModule{T}"/> implementations.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed parameters for the associated dialog.</typeparam>
    public interface ICustomDialog<T> : IDataContextDialog<T> where T : DialogProperties, new()
    {
        /// <summary>
        /// A reference to the module whose DataContext is this instance. 
        /// </summary>
        new ICustomDialogModule<T> DialogModule { get; set; }

        IDataContextDialogModule<T> IDataContextDialog<T>.DialogModule
        {
            get => DialogModule;
            set
            {
                if (value is ICustomDialogModule<T> typedModule)
                {
                    DialogModule = typedModule;
                }
                else
                {
                    throw new InvalidCastException($"Cannot assign module of different type than {typeof(ICustomDialogModule<T>)}" +
                        $"because this dialog backend is {typeof(ICustomDialog<T>)}.");
                }
            }
        }

        new CustomDialogControl<T> DialogControl { get; }

        DataContextDialogControl<T> IDataContextDialog<T>.DialogControl => DialogControl;
    }
}
