﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mocha.Dialogs
{
    /// <summary>
    /// Register, fetch and manage your dialogs with this class.
    /// </summary>
    public static class DialogManager
    {
        private static readonly Dictionary<string, Func<IDialogModule>> _dictionary = new Dictionary<string, Func<IDialogModule>>();
        private static readonly Dictionary<string, List<IDialogModule>> _activeDialogs = new Dictionary<string, List<IDialogModule>>();

        /// <summary>
        /// Allows to register a <see cref="IDialogModule"/>.
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
        /// Retrieves a registered dialog by its Identifier.
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

            dialog.Closed += OnDialogClosed;
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
