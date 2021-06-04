using Mocha.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocha
{
    /// <summary>
    /// Sometimes you don't need a custom logic for particular dialog. Use <see cref="SimpleDialogData"/>
    /// to satisfy backend requirement for <see cref="IDialogModule"/> when no specific backend is needed.
    /// </summary>
    public class SimpleDialogData : IDialog
    {
        /// <summary>
        /// Exposes API for dialog interaction.
        /// </summary>
        public DialogControl DialogControl { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDialogData"/> class. 
        /// </summary>
        public SimpleDialogData()
        {
            DialogControl = new DialogControl(this);
        }
    }

    /// <summary>
    /// Sometimes you don't need a custom logic for particular dialog. Use <see cref="SimpleDialogData"/>
    /// to satisfy backend requirement for <see cref="IDialogModule"/> when no specific backend is needed.
    /// </summary>
    /// <typeparam name="T">Specifies statically typed parameters for the associated dialog.</typeparam>
    public class SimpleDialogData<T> : IDialog<T> where T : DialogControl
    {
        /// <summary>
        /// Exposes API for dialog interaction.
        /// </summary>
        public T DialogControl { get; }

        /// <summary>
        /// Exposes API for dialog interaction.
        /// </summary>
        DialogControl IDialog.DialogControl => DialogControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDialogData{T}"/> class. 
        /// </summary>
        public SimpleDialogData()
        {
            try
            {
                DialogControl = (T)Activator.CreateInstance(typeof(T), this);
            }
            catch (MissingMethodException ex)
            {
                throw new InvalidOperationException($"Cannot create instance of {typeof(T)} because it does not have a constructor which takes one parameter of type IDialog<{typeof(T)}>. Either add descirbed constructor to your custom type which derives from {typeof(T).BaseType} class or do not pass null as dataContext for BaseDialogModule constructor.", ex);
            }
        } 
    }
}
