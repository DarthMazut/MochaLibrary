using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mocha.Dialogs
{
    /// <summary>
    /// Exposes API for dialog interaction.
    /// </summary>
    public class DialogControl : IDisposable
    {
        private readonly Dictionary<string, object> _customParameterDictionary = new Dictionary<string, object>();
        private readonly IDialog _dialog;
        private IDialogModule _module;
        private Action _closeAction;
        
        /// <summary>
        /// Describes the result of dialog interaction.
        /// </summary>
        public bool? DialogResult { get; set; }

        /// <summary>
        /// Value retrieved as a result of dialog interaction. 
        /// </summary>
        public object DialogValue { get; set; }

        /// <summary>
        /// Parent element which called a dialog parametrized by this instance.
        /// Setting this to <see langword="null"/> means MainWindow is consider a parent.
        /// </summary>
        public IDialog Parent { get; set; }

        /// <summary>
        /// Title for displaying dialog.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Message for displaying dialog.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Icon for displaying dialog.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Defines a predefined buttons set for displaying dialog.
        /// </summary>
        public string PredefinedButtons { get; set; }

        /// <summary>
        /// Defines a filter for Open/Save dialog.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Sets initial directory for Open/Save dialog.
        /// </summary>
        public string InitialDirectory { get; set; }

        /// <summary>
        /// Extension added automatically in case none was specified.
        /// </summary>
        public string DefaultExtension { get; set; }

        /// <summary>
        /// Allows for storage of custom parameters.
        /// </summary>
        public Dictionary<string, object> Dictionary => _customParameterDictionary;

        /// <summary>
        /// Fires when dialog is displayed.
        /// </summary>
        public event EventHandler Opening;

        /// <summary>
        /// Fires when dialog is about to close.
        /// <para> Not every implementation of <see cref="IDialogModule"/> supports this event ! </para>
        /// </summary>
        public event EventHandler<CancelEventArgs> Closing;

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
        /// Closes the dialog if open.
        /// </summary>
        public void Close()
        {
            _closeAction.Invoke();
        }

        /// <summary>
        /// Perform cleaning operations allowing this object to be garbage collected.
        /// </summary>
        public void Dispose()
        {
            UnsubscribeAll();
        }

        internal void SetBehaviours(IDialogModule module)
        {
            _closeAction = module.Close;
        }

        internal void ActivateSubscribtion()
        {
            _module = FindCorrespondingModule(_dialog);
            SubscribeAll();
        }

        private IDialogModule FindCorrespondingModule(IDialog dialog)
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
            if (_module != null)
            {
                _module.Closing += OnClosing;
                _module.Opening += OnDisplayed;
            }
        }

        private void UnsubscribeAll()
        {
            if (_module != null)
            {
                _module.Closing -= OnClosing;
                _module.Opening -= OnDisplayed;
            }
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            Closing?.Invoke(this, e);
        }

        private void OnDisplayed(object sender, EventArgs e)
        {
            Opening?.Invoke(this, e);
        }
    }
}
