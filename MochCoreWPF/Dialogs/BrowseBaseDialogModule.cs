using MochaCore.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MochCoreWPF.Dialogs
{
    /// <summary>
    /// Provides base class for *OpenFile*, *SaveFile* and *BrowseFolder* dialog modules.
    /// </summary>
    /// <typeparam name="TView">Type of underlying dialog object.</typeparam>
    /// <typeparam name="TResult">Technology specific result type of underlying dialog object.</typeparam>
    /// <typeparam name="TProperties">Type of statically typed properties for represented dialog.</typeparam>
    public abstract class BrowseBaseDialogModule<TView, TResult, TProperties> : IDialogModule<TProperties> where TProperties : DialogProperties, new()
    {
        private readonly Window _mainWindow;
        private readonly TView _view;

        /// <summary>
        /// Provides base constructor for descendants of <see cref="BrowseBaseDialogModule{TView, TResult, TProperties}"/>.
        /// </summary>
        /// <param name="application">WPF Application object.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        /// <param name="view">Technology-specific dialog object.</param>
        public BrowseBaseDialogModule(Application application, TProperties properties, TView view)
        {
            _mainWindow = application.MainWindow;
            _view = view;

            Properties = properties;

            ApplyProperties = ApplyPropertiesCore;
            ShowDialog = ShowDialogCore;
            HandleResult = HandleResultCore;
            FindParent = FindParentCore;
        }

        /// <inheritdoc/>
        public TProperties Properties { get; set; }

        /// <inheritdoc/>
        public object? View => _view;

        /// <summary>
        /// Applies <see cref="Properties"/> values to technology-specific dialog object.
        /// </summary>
        public Action<TView, TProperties> ApplyProperties { get; set; }

        /// <summary>
        /// Handles technology-specific process of dialog show.
        /// </summary>
        public Func<TView, Window, TResult> ShowDialog { get; set; }

        /// <summary>
        /// Translates technology-specific dialog result into technolog-independant value.
        /// Sets suitable properties in <see cref="Properties"/> if required.
        /// </summary>
        public Func<TView, TResult, TProperties, bool?> HandleResult { get; set; }

        /// <summary>
        /// Handles the process of search for parent <see cref="Window"/> for technology-specific dialog object.
        /// </summary>
        public Func<object, Window> FindParent { get; set; }

        /// <inheritdoc/>
        public event EventHandler? Opening;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <summary>
        /// Satisfies <see cref="IDisposable"/> interface.
        /// In this particular case no resources are explicitly freed.
        /// </summary>
        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public Task<bool?> ShowModalAsync(object host)
        {
            ApplyProperties.Invoke(_view, Properties);

            Opening?.Invoke(this, EventArgs.Empty);
            bool? result = HandleResult.Invoke(_view, ShowDialog.Invoke(_view, FindParent.Invoke(host)), Properties);
            Closed?.Invoke(this, EventArgs.Empty);

            return Task.FromResult(result);
        }

        /// <summary>
        /// Applies <see cref="Properties"/> values to technology-specific dialog object.
        /// </summary>
        /// <param name="dialog">Technology-specific dialog object.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        protected abstract void ApplyPropertiesCore(TView dialog, TProperties properties);

        /// <summary>
        /// Handles technology-specific process of dialog show.
        /// </summary>
        /// <param name="dialog">Technology-specific dialog object.</param>
        /// <param name="parent">Parent <see cref="Window"/> for technology-specific dialog object.</param>
        protected abstract TResult ShowDialogCore(TView dialog, Window parent);

        /// <summary>
        /// Translates technology-specific dialog result into technolog-independant value.
        /// Sets suitable properties in <see cref="Properties"/> if required.
        /// </summary>
        /// <param name="dialog">Technology-specific dialog object.</param>
        /// <param name="result">Technology-specific result of dialog interaction.</param>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        protected abstract bool? HandleResultCore(TView dialog, TResult result, TProperties properties);

        /// <summary>
        /// Handles the process of search for parent <see cref="Window"/> for technology-specific dialog object.
        /// </summary>
        /// <param name="host">Object which parent window is to be found.</param>
        protected virtual Window FindParentCore(object host)
        {
            return ParentResolver.FindParent<TProperties>(host) ?? _mainWindow;
        }

    }
}
