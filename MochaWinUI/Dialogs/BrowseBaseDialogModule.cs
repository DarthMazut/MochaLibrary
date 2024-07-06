using Microsoft.UI.Xaml;
using MochaCore.Dialogs;
using MochaWinUI.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace MochaWinUI.Dialogs
{
    /// <summary>
    /// Provides a base class for <see cref="OpenFileDialogModule"/>, <see cref="SaveFileDialogModule"/> and <see cref="BrowseFolderDialogModule"/>.
    /// </summary>
    /// <typeparam name="TView">Type of underlying dialog object.</typeparam>
    /// <typeparam name="TResult">Technology specific result type of underlying dialog object.</typeparam>
    /// <typeparam name="TProperties">Type of statically typed properties for represented dialog.</typeparam>
    public abstract class BrowseBaseDialogModule<TView, TResult, TProperties> : IDialogModule<TProperties>
        where TProperties : new()
        where TView : class
    {
        private readonly TView _view;

        /// <summary>
        /// Provides base constructor for descendants of <see cref="BrowseBaseDialogModule{TView, TResult, TProperties}"/>.
        /// </summary>
        /// <param name="properties">Statically typed properties object which serves for configuration of this module.</param>
        /// <param name="view">Technology-specific dialog object.</param>
        public BrowseBaseDialogModule(TProperties properties, TView view)
        {
            _view = view;
            Properties = properties;

            ApplyProperties = ApplyPropertiesCore;
            ShowDialog = ShowDialogCore;
            HandleResult = HandleResultCore;
            FindParent = FindParentCore;
            DisposeDialog = DisposeDialogCore;
        }

        /// <inheritdoc/>
        public TProperties Properties { get; set; }

        /// <inheritdoc/>
        public object View => _view;

        /// <inheritdoc/>
        public event EventHandler? Opening;

        /// <inheritdoc/>
        public event EventHandler? Closed;

        /// <inheritdoc/>
        public event EventHandler? Disposed;

        /// <summary>
        /// Applies <see cref="Properties"/> values to technology-specific dialog object.
        /// </summary>
        public Action<TView, TProperties> ApplyProperties { get; set; }

        /// <summary>
        /// Handles technology-specific process of dialog show.
        /// </summary>
        public Func<TView, Task<TResult>> ShowDialog { get; set; }

        /// <summary>
        /// Translates technology-specific dialog result into technolog-independant value.
        /// Sets suitable properties in <see cref="Properties"/> if required.
        /// </summary>
        public Func<TView, TResult, TProperties, bool?> HandleResult { get; set; }

        /// <summary>
        /// Handles the process of search for parent <see cref="Window"/> for technology-specific dialog object.
        /// </summary>
        public Func<object?, Window> FindParent { get; set; }

        /// <summary>
        /// Allows for providing a custom code to be executed while this object is being disposed of.
        /// Use this delegate when there are disposable resources within your custom <see cref="Properties"/> object.
        /// </summary>
        public Action<BrowseBaseDialogModule<TView, TResult, TProperties>> DisposeDialog { get; set; }

        /// <summary>
        /// Diffrientiates between diffrent pickers.
        /// </summary>
        public string? DialogIdentifier { get; init; }

        /// <inheritdoc/>
        public void Dispose()
        {
            DisposeDialog.Invoke(this);
            GC.SuppressFinalize(this);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public async Task<bool?> ShowModalAsync(object? host)
        {
            ApplyProperties.Invoke(_view, Properties);
            WorkaroundForBug466(FindParent.Invoke(host));
            
            Opening?.Invoke(this, EventArgs.Empty);
            bool? result = HandleResult.Invoke(_view, await ShowDialog.Invoke(_view), Properties);
            Closed?.Invoke(this, EventArgs.Empty);

            return result;
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
        protected abstract Task<TResult> ShowDialogCore(TView dialog);

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
        protected virtual Window FindParentCore(object? host)
        {
            if (ParentResolver.FindParentWindow(host) is Window foundWindow)
            {
                return foundWindow;
            }

            throw new NotImplementedException(
                $"The default implementation of {GetType().Name} could not resolve the parent of the provided object. " +
                $"In this case, you need to provide your own implementation of {nameof(FindParent)} either by supplying a custom " +
                $"{nameof(FindParent)} delegate or by subclassing {GetType().Name} and overriding the {nameof(FindParentCore)} method."
            );
        }

        /// <summary>
        /// Allows for providing a custom code to be executed while this object is being disposed of.
        /// Override this when there are disposable resources within your custom <see cref="Properties"/> object.
        /// </summary>
        /// <param name="module">Module that's being disposed.</param>
        protected virtual void DisposeDialogCore(BrowseBaseDialogModule<TView, TResult, TProperties> module) { }

        /// <summary>
        /// Maps the values of the <see cref="Environment.SpecialFolder"/> enum to their <see cref="PickerLocationId"/> equivalents.
        /// </summary>
        /// <param name="folder">The <see cref="Environment.SpecialFolder"/> value to be mapped.</param>
        protected static PickerLocationId MapSpecialFolderToLocationId(Environment.SpecialFolder? folder)
        {
            Dictionary<Environment.SpecialFolder, PickerLocationId> locationMap = new()
            {
                {Environment.SpecialFolder.MyDocuments, PickerLocationId.DocumentsLibrary},
                {Environment.SpecialFolder.MyComputer, PickerLocationId.ComputerFolder},
                {Environment.SpecialFolder.Desktop, PickerLocationId.Desktop},
                {Environment.SpecialFolder.MyMusic, PickerLocationId.MusicLibrary},
                {Environment.SpecialFolder.MyPictures, PickerLocationId.PicturesLibrary},
                {Environment.SpecialFolder.MyVideos, PickerLocationId.VideosLibrary}
            };

            if (folder.HasValue && locationMap.TryGetValue(folder.Value, out PickerLocationId locationId))
            {
                return locationId;
            }

            return PickerLocationId.Unspecified;
        }

        // Workaround for bug https://github.com/microsoft/WindowsAppSDK/issues/466
        private void WorkaroundForBug466(object parent)
        {
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(parent);
            WinRT.Interop.InitializeWithWindow.Initialize(_view, hwnd);
        }
    }
}
