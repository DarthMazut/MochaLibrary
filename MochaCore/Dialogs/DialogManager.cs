using System;
using System.Collections.Generic;
using System.Linq;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Allows for defining, retrieving and managing dialogs.
    /// </summary>
    public static class DialogManager
    {
        private static readonly Dictionary<string, Func<IDialogModule>> _dictionary = new();
        private static readonly Dictionary<string, List<IDialogModule>> _activeDialogs = new();

        /// <summary>
        /// Allows to register a <see cref="IDialogModule"/>. Dialogs registered by this method can
        /// be tracked and obtained via <see cref="GetActiveDialogs(string)"/> method.
        /// </summary>
        /// <param name="id">Identifier which allows the technology-independent layer retrieve this dialog.</param>
        /// <param name="dialogDelegate">This delegate should return new instance of <see cref="IDialogModule"/> object.</param>
        public static void DefineDialog(string id, Func<IDialogModule> dialogDelegate)
        {
            if(_dictionary.ContainsKey(id))
            {
                throw new InvalidOperationException($"Cannot define a dialog with ID={id} because it is already defined.");
            }

            _dictionary.Add(id, dialogDelegate);
        }

        /// <summary>
        /// Retrieves a registered dialog by its identifier.
        /// </summary>
        /// <param name="id">Identifier of registered dialog to be retrieved.</param>
        public static IDialogModule GetDialog(string id)
        {
            if(_dictionary.ContainsKey(id))
            {
                IDialogModule? dialog = _dictionary[id]?.Invoke();
                HandleCreatedDialog(id, dialog);
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
        public static IDialogModule<T> GetDialog<T>(string id) where T : DialogControl
        {
            return (IDialogModule<T>)GetDialog(id);
        }

        /// <summary>
        /// Returns a collection of references to created dialogs, which hasn't been disposed yet.
        /// </summary>
        /// <param name="id">Identifier of specific dialog.</param>
        public static List<IDialogModule> GetActiveDialogs(string id)
        {
            if (_activeDialogs.ContainsKey(id))
            {
                return _activeDialogs[id];
            }

            return new List<IDialogModule>();
        }

        /// <summary>
        /// Returns a collection of all <see cref="IDialogModule"/>, which has been 
        /// requested by <see cref="GetDialog(string)"/> method but not yet disposed.
        /// </summary>
        public static List<IDialogModule> GetActiveDialogs()
        {
            return _activeDialogs.Where(kvp => kvp.Value.Any()).SelectMany(kvp => kvp.Value).ToList();
        }

        private static void HandleCreatedDialog(string id, IDialogModule? dialog)
        {
            _ = dialog ?? throw new ArgumentNullException(nameof(dialog), $"Dialog creation delegate with id {id} returned null. " +
                    "Make sure that delegate which creates dialog is properly defined.");

            if (_activeDialogs.ContainsKey(id))
            {
                _activeDialogs[id].Add(dialog);
            }
            else
            {
                _activeDialogs.Add(id, new List<IDialogModule>() { dialog });
            }

            dialog.Disposed += (s, e) =>
            {
                if (_activeDialogs.ContainsKey(id))
                {
                    if (s is IDialogModule dialogModule)
                    {
                        _activeDialogs[id].Remove(dialogModule);
                    }
                    else
                    {
                        ArgumentNullException ex = new(nameof(dialog),
                            "Bad IDialogModule implementation. Disposed event handler did not provide IDialogModule as sender. " +
                            "See Data of this exception to obtain reference to faulted implementation of IDialogModule.");
                        
                        ex.Data.Add("faultedModule", dialog);
                        throw ex;
                    }
                }
            };
        }
    }
}
