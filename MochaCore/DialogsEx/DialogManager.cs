using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.DialogsEx
{
    /// <summary>
    /// Allows for defining, retrieving and managing dialogs.
    /// </summary>
    public static class DialogManager
    {
        private static readonly Dictionary<string, Func<IDialogModule>> _dialogsDictionary = new();

        /// <summary>
        /// Allows to register a <see cref="IDialogModule"/>. Dialogs registered by this method can
        /// be tracked and obtained via <see cref="GetActiveDialogs(string)"/> method.
        /// </summary>
        /// <param name="id">Identifier which allows the technology-independent layer retrieve this dialog.</param>
        /// <param name="dialogDelegate">This delegate should return new instance of <see cref="IDialogModule"/> object.</param>
        public static void DefineDialog(string id, Func<IDialogModule> dialogDelegate)
        {
            if (_dialogsDictionary.ContainsKey(id))
            {
                throw new InvalidOperationException($"Cannot define a dialog with ID={id} because it is already defined.");
            }

            _dialogsDictionary.Add(id, dialogDelegate);
        }

        /// <summary>
        /// Retrieves a registered dialog by its identifier.
        /// </summary>
        /// <param name="id">Identifier of registered dialog to be retrieved.</param>
        public static IDialogModule GetDialog(string id)
        {
            if (_dialogsDictionary.ContainsKey(id))
            {
                IDialogModule? dialog = _dialogsDictionary[id]?.Invoke();
                //HandleCreatedDialog(id, dialog);
                return dialog!;
            }
            else
            {
                throw new ArgumentException($"Couldn't find dialog with ID {id}. Make sure ID is correct and dialog is defined.");
            }
        }

        /// <summary>
        /// Creates an instance of specified <see cref="IDialogModule{T}"/> type.
        /// </summary>
        /// <typeparam name="T">Type of internal <see cref="DialogControl"/> object.</typeparam>
        /// <param name="id">Specifies the dialog identifier.</param>
        public static IDialogModule<T> GetDialog<T>(string id) where T : DialogProperties
        {
            return (IDialogModule<T>)GetDialog(id);
        }
    }
}
