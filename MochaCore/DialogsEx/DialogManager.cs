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
        private static readonly Dictionary<string, List<IDialogModule>> _activeDialogs = new();

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
        /// Retrieves an instance of registered <see cref="IDialogModule{T}"/> type by its identifier.
        /// </summary>
        /// <typeparam name="T">Type of internal <see cref="DialogProperties"/> object.</typeparam>
        /// <param name="id">Identifier of registered dialog to be retrieved.</param>
        public static IDialogModule<T> GetDialog<T>(string id)
        {
            return (IDialogModule<T>)GetDialogCore(id);
        }

        /// <summary>
        /// Retrieves an instance of registered <see cref="IDataContextDialogModule{T}"/> type by its identifier.
        /// </summary>
        /// <typeparam name="T">Type of internal <see cref="DialogProperties"/> object.</typeparam>
        /// <param name="id">Identifier of registered dialog to be retrieved.</param>
        public static IDataContextDialogModule<T> GetDataContextDialog<T>(string id)
        {
            return (IDataContextDialogModule<T>)GetDialogCore(id);
        }

        /// <summary>
        /// Retrieves an instance of registered <see cref="ICustomDialogModule{T}"/> type by its identifier.
        /// </summary>
        /// <typeparam name="T">Type of internal <see cref="DialogProperties"/> object.</typeparam>
        /// <param name="id">Identifier of registered dialog to be retrieved.</param>
        public static ICustomDialogModule<T> GetCustomDialog<T>(string id)
        {
            return (ICustomDialogModule<T>)GetDialogCore(id);
        }

        // <summary>
        // Retrieves an instance of registered <see cref="IDialogModule"/> type by its identifier.
        // It is not recommended to use this non-generic method. Use *, * or * instead.
        // </summary>
        // <param name="id">Identifier of registered dialog to be retrieved.</param>
        private static IDialogModule GetDialogCore(string id)
        {
            if (_dialogsDictionary.ContainsKey(id))
            {
                return HandleCreateDialog(id);
            }
            else
            {
                throw new ArgumentException($"Couldn't find dialog with ID {id}. Make sure ID is correct and dialog is defined.");
            }
        }

        /// <summary>
        /// Returns a collection of references to opened dialogs, which hasn't been disposed yet.
        /// </summary>
        /// <param name="id">Identifier of specific dialog.</param>
        public static List<IDialogModule> GetOpenedDialogs(string id)
        {
            if (_activeDialogs.ContainsKey(id))
            {
                return _activeDialogs[id];
            }

            return new List<IDialogModule>();
        }

        /// <summary>
        /// Returns a collection of all <see cref="IDialogModule"/>, which has been opened but not yet disposed.
        /// </summary>
        public static List<IDialogModule> GetOpenedDialogs()
        {
            return _activeDialogs.Where(kvp => kvp.Value.Any()).SelectMany(kvp => kvp.Value).ToList();
        }

        private static IDialogModule HandleCreateDialog(string id)
        {
            IDialogModule? dialog = _dialogsDictionary[id]?.Invoke();
            if (dialog is null)
            {
                throw new NullReferenceException($"Dialog creation delegate with id {id} returned null. " +
                    $"Make sure that delegate which creates dialog is properly defined.");
            }

            dialog.Opening += (s, e) =>
            {
                if (_activeDialogs.ContainsKey(id))
                {
                    _activeDialogs[id].Add(dialog);
                }
                else
                {
                    _activeDialogs.Add(id, new List<IDialogModule>() { dialog });
                }
            };

            dialog.Disposed += (s, e) => 
            {
                if (_activeDialogs.ContainsKey(id))
                {
                    _activeDialogs[id].Remove(dialog);
                }
            };

            return dialog;
        }
    }
}
