using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Exposes API for dialog interaction.
    /// </summary>
    public class DialogControl : IDisposable
    {
        private readonly Dictionary<string, object?> _parameters = new();
        private readonly IDialog _dialog;
        
        private IDialogModule? _module;
        private Action? _closeAction;
        
        /// <summary>
        /// Describes the result of dialog interaction.
        /// </summary>
        public bool? DialogResult { get; set; }

        /// <summary>
        /// Parent element which called a dialog parametrized by this instance.
        /// Setting this to <see langword="null"/> means MainWindow is consider a parent.
        /// </summary>
        public IDialog? Parent { get; set; }

        /// <summary>
        /// Title for displaying dialog.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Allows for storage of custom parameters.
        /// </summary>
        public Dictionary<string, object?> Parameters => _parameters;

        /// <summary>
        /// Fires when dialog is displayed.
        /// </summary>
        public event EventHandler? Opening;

        /// <summary>
        /// Fires right after the dialog opens.
        /// <para> Not every implementation of <see cref="IDialogModule"/> can supports this event ! </para>
        /// </summary>
        public event EventHandler? Opened;

        /// <summary>
        /// Fires when dialog is about to close.
        /// <para> Not every implementation of <see cref="IDialogModule"/> supports this event ! </para>
        /// </summary>
        public event EventHandler<CancelEventArgs>? Closing;

        /// <summary>
        /// Returns new instance of <see cref="DialogControl"/> class.
        /// </summary>
        /// <param name="dialog">
        /// An <see cref="IDialog"/> implementation which hosts this instance.
        /// Pass <see langword="this"/> here.
        /// </param>
        public DialogControl(IDialog dialog)
        {
            _dialog = dialog;
        }

        /// <summary>
        /// Tries to fetch parameter corresponding to specified key from
        /// <see cref="Parameters"/> dictionary. Returns default value of
        /// expected type in case of type mismatch or no parameter was found.
        /// </summary>
        /// <typeparam name="T">Expected type of retrieving value.</typeparam>
        /// <param name="key">Parameter key.</param>
        public T? GetParameter<T>(string key)
        {
            if (Parameters.TryGetValue(key, out object? parameter))
            {
                if (parameter is T value)
                {
                    return value;
                }
            }

            return default;
        }

        /// <summary>
        /// Adds or updates specified parameter within <see cref="Parameters"/> dictionary.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <param name="value">Value to be added or updated.</param>
        public void SetParameter(string key, object? value)
        {
            if (Parameters.ContainsKey(key))
            {
                Parameters[key] = value;
            }
            else
            {
                Parameters.Add(key, value);
            }
        }

        /// <summary>
        /// Closes the dialog if open.
        /// </summary>
        public void Close()
        {
            _closeAction?.Invoke();
        }

        /// <summary>
        /// Perform cleaning operations allowing this object to be garbage collected.
        /// </summary>
        public void Dispose()
        {
            UnsubscribeAll();
            _closeAction = null;

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs operations to bind specified <see cref="IDialogModule"/> with this instance.
        /// </summary>
        /// <param name="module">Module to bind with this instance.</param>
        public void Activate(IDialogModule module)
        {
            _closeAction = module.Close;
            _module = FindCorrespondingModule(_dialog);
            SubscribeAll();
        }

        private IDialogModule? FindCorrespondingModule(IDialog dialog)
        {
            foreach (IDialogModule module in DialogManager.GetActiveDialogs())
            {
                if (module.DataContext == dialog)
                {
                    return module;
                }
            }

            return null;
        }

        private void SubscribeAll()
        {
            // This ensures that even when Activate() is called multiple times
            // we do not end up with redundant subscribtions
            UnsubscribeAll(); 

            if (_module != null)
            {
                _module.Closing += OnClosing;
                _module.Opening += OnDisplayed;
                _module.Opened += OnOpened;
            }
        }

        private void UnsubscribeAll()
        {
            if (_module != null)
            {
                _module.Closing -= OnClosing;
                _module.Opening -= OnDisplayed;
                _module.Opened -= OnOpened;
            }
        }

        private void OnClosing(object? sender, CancelEventArgs e)
        {
            Closing?.Invoke(this, e);
        }

        private void OnDisplayed(object? sender, EventArgs e)
        {
            Opening?.Invoke(this, e);
        }

        private void OnOpened(object? sender, EventArgs e)
        {
            Opened?.Invoke(this, e);
        }
    }
}
