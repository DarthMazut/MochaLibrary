using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocha.Dialogs
{
    /// <summary>
    /// Allows for defining, retrieving and managing custom dialogs. 
    /// Provides methods for obtaining standarized dialog instances.
    /// </summary>
    public static class DialogManager
    {
        private static readonly Dictionary<string, Func<IDialogModule>> _dictionary = new Dictionary<string, Func<IDialogModule>>();
        private static readonly Dictionary<string, List<IDialogModule>> _activeDialogs = new Dictionary<string, List<IDialogModule>>();
        private static Dictionary<string, IDialogFactory> _factoriesDictionary = new Dictionary<string, IDialogFactory>();
        private static IDialogFactory _defaultFactory;

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
                IDialogModule dialog = _dictionary[id]?.Invoke();
                HandleCreatedDialog(dialog);
                return dialog;
            }
            else
            {
                throw new ArgumentException($"Couldn't find dialog with ID {id}. Make sure ID is correct and dialog is defined.");
            }
        }

        /// <summary>
        /// Sets the default <see cref="IDialogFactory"/>.
        /// </summary>
        /// <param name="factory">Default factory.</param>
        public static void SetDefaultFactory(IDialogFactory factory)
        {
            _defaultFactory = factory ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Register a <see cref="IDialogFactory"/> which provides instances 
        /// of <see cref="IDialogModule"/> for technology-independant layer.
        /// </summary>
        /// <param name="factoryID">Identifier for given factory.</param>
        /// <param name="factory">Factory to be registered.</param>
        public static void AddFactory(string factoryID, IDialogFactory factory)
        {
            if (_factoriesDictionary.ContainsKey(factoryID))
            {
                throw new InvalidOperationException($"Cannot add factory with ID={factoryID} because it was already added.");
            }

            _factoriesDictionary.Add(factoryID, factory);
        }

        /// <summary>
        /// Returns <see cref="IDialogModule"/> created by default factory.
        /// </summary>
        /// <param name="parameters">Configuration parameters used by default factory.</param>
        public static IDialogModule GetDialogFromFactory(params string[] parameters)
        {
            if (_defaultFactory == null) throw new InvalidOperationException("Cannot use _defaultFacotry because it wasn't set.");

            return _defaultFactory.Create(parameters);
        }

        /// <summary>
        /// Returns a <see cref="IDialogModule"/> created by factory corresponding to provided identifier.
        /// </summary>
        /// <param name="factoryID">Identifier of chosen factory.</param>
        /// <param name="parameters">Configuration parameters used by selected factory.</param>
        public static IDialogModule GetDialogFromFactory(string factoryID, params string[] parameters)
        {
            if (factoryID == null) throw new ArgumentNullException();

            if (_factoriesDictionary.ContainsKey(factoryID))
            {
                return _factoriesDictionary[factoryID].Create(parameters);
            }

            throw new ArgumentException($"Cannot find factory with ID={factoryID}. Make sure it was added first.");
        }

        /// <summary>
        /// Returns a collection of references to created dialogs, which hasn't been disposed yet.
        /// </summary>
        /// <param name="id">Identifier of specific dialog.</param>
        public static List<IDialogModule> GetActiveDialogs(string id)
        {
            if (_dictionary.ContainsKey(id))
            {
                IDialogModule dialog = _dictionary[id]?.Invoke();
                string key = dialog.View.GetType().ToString();

                if(_activeDialogs.ContainsKey(key))
                {
                    return _activeDialogs[key];
                }
            }

            return new List<IDialogModule>();
        }

        private static void HandleCreatedDialog(IDialogModule dialog)
        {
            string key = dialog.View.GetType().ToString();

            if (_activeDialogs.ContainsKey(key))
            {
                _activeDialogs[key].Add(dialog);
            }
            else
            {
                _activeDialogs.Add(key, new List<IDialogModule>() { dialog });
            }

            dialog.Disposed += OnDialogClosed;
        }

        private static void OnDialogClosed(object sender, EventArgs e)
        {
            string key = (sender as IDialogModule).View.GetType().ToString();

            if (_activeDialogs.ContainsKey(key))
            {
                _activeDialogs[key].Remove(sender as IDialogModule);
            }
        }
    }
}
