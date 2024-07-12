using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Allows for defining, retrieving and managing dialogs.
    /// </summary>
    public static class DialogManager
    {
        private static readonly Dictionary<string, Func<IDialogModule>> _dialogsDictionary = new();
        private static readonly Dictionary<string, List<IDialogModule>> _activeDialogs = new();

        /// <summary>
        /// Allows to register an <see cref="IDialogModule"/>. Dialogs registered by this method can
        /// be tracked and obtained via <see cref="GetOpenedDialogs()"/> method.
        /// </summary>
        /// <param name="id">Identifier which allows the technology-independent layer retrieve this dialog.</param>
        /// <param name="dialogDelegate">This delegate should return new instance of <see cref="IDialogModule"/> object.</param>
        public static void RegisterDialog(string id, Func<IDialogModule> dialogDelegate)
        {
            if (_dialogsDictionary.ContainsKey(id))
            {
                throw new InvalidOperationException($"Cannot define a dialog with ID={id} because it is already defined.");
            }

            _dialogsDictionary.Add(id, dialogDelegate);
        }

        /// <summary>
        /// Retrieves a new instance of <see cref="IDialogModule"/> registered by the provided identifier.
        /// </summary>
        /// <param name="id">Identifier of the registered <see cref="IDialogModule"/> to be retrieved.</param>
        public static IDialogModule RetrieveDialog(string id)
            => GetBaseDialog(id);

        /// <summary>
        /// Retrieves a new instance of <see cref="IDialogModule{T}"/> registered by the provided identifier.
        /// </summary>
        /// <typeparam name="T">The statically typed properties object associated with the module.</typeparam>
        /// <param name="id">Identifier of the registered <see cref="IDialogModule{T}"/> to be retrieved.</param>
        public static IDialogModule<T> RetrieveDialog<T>(string id) where T : new()
            => (IDialogModule<T>)GetBaseDialog(id);

        /// <summary>
        /// Retrieves a new instance of <see cref="IDataContextDialogModule"/> registered by the provided identifier.
        /// </summary>
        /// <param name="id">Identifier of the registered <see cref="IDataContextDialogModule"/> to be retrieved.</param>
        public static IDataContextDialogModule RetrieveDataContextDialog(string id)
            => (IDataContextDialogModule)GetBaseDialog(id);

        /// <summary>
        /// Retrieves a new instance of <see cref="IDataContextDialogModule{T}"/> registered by the provided identifier.
        /// </summary>
        /// <typeparam name="T">The statically typed properties object associated with the module.</typeparam>
        /// <param name="id">Identifier of the registered <see cref="IDataContextDialogModule{T}"/> to be retrieved.</param>
        public static IDataContextDialogModule<T> RetrieveDataContextDialog<T>(string id) where T : new()
            => (IDataContextDialogModule<T>)GetBaseDialog(id);

        /// <summary>
        /// Retrieves a new instance of <see cref="ICustomDialogModule"/> registered by the provided identifier.
        /// </summary>
        /// <param name="id">Identifier of the registered <see cref="ICustomDialogModule"/> to be retrieved.</param>
        public static ICustomDialogModule RetrieveCustomDialog(string id) => (ICustomDialogModule)GetBaseDialog(id);

        /// <summary>
        /// Retrieves a new instance of <see cref="ICustomDialogModule{T}"/> registered by the provided identifier.
        /// </summary>
        /// <typeparam name="T">The statically typed properties object associated with the module.</typeparam>
        /// <param name="id">Identifier of the registered <see cref="ICustomDialogModule{T}"/> to be retrieved.</param>
        public static ICustomDialogModule<T> RetrieveCustomDialog<T>(string id) where T :  new()
            => (ICustomDialogModule<T>)GetBaseDialog(id);

        /// <summary>
        /// Returns a collection of all <see cref="IDialogModule"/>, which has been opened but not yet disposed.
        /// </summary>
        public static IReadOnlyCollection<IDialogModule> GetOpenedDialogs()
        {
            return _activeDialogs.Where(kvp => kvp.Value.Any()).SelectMany(kvp => kvp.Value).ToImmutableList();
        }

        /// <summary>
        /// Returns a collection of references to opened dialogs, which hasn't been disposed yet.
        /// </summary>
        /// <param name="id">Identifier of specific dialog.</param>
        public static IReadOnlyCollection<IDialogModule> GetOpenedDialogs(string id)
        {
            if (_activeDialogs.ContainsKey(id))
            {
                return _activeDialogs[id].ToImmutableList();
            }

            return new List<IDialogModule>().ToImmutableList();
        }

        private static IDialogModule GetBaseDialog(string id)
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
