﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Provides technology-agnostic abstraction over technology-specific window instance and its data context.
    /// </summary>
    public interface IBaseWindowModule : IDisposable
    {
        /// <summary>
        /// Returns the technolog-specific window object.
        /// </summary>
        public object View { get; }

        /// <summary>
        /// Returns the data context of the representing window or <see langword="null"/> if no such was specified.
        /// </summary>
        public IBaseWindowAware? DataContext { get; }

        /// <summary>
        /// Indicates whether associated window is open.
        /// </summary>
        public bool IsOpen { get; }

        /// <summary>
        /// Inidicates whether current instance was already disposed.
        /// Disposed <see cref="IBaseWindowModule"/> objects should no longer be interacted with.
        /// </summary>
        public bool IsDisposed { get; }

        /// <summary>
        /// Occurs when representing window is opened.
        /// </summary>
        public event EventHandler? Opened;

        /// <summary>
        /// Occurs when representing window closes.
        /// </summary>
        public event EventHandler? Closed;

        /// <summary>
        /// Occurs when representing window is disposed.
        /// </summary>
        public event EventHandler? Disposed;

        /// <summary>
        /// Opens the representing window.
        /// </summary>
        public void Open();

        /// <summary>
        /// Opens the representing window.
        /// </summary>
        /// <param name="parentModule">Module to be parent of the representing window.</param>
        public void Open(IBaseWindowModule parentModule);

        /// <summary>
        /// Opens the representing window.
        /// </summary>
        /// <param name="parent">Object to be parent of the representing window.</param>
        public void Open(object parent);

        /// <summary>
        /// Opens the representing window and returns <see cref="Task"/> which completes
        /// when opened window is closed.
        /// </summary>
        public Task<object?> OpenAsync();

        /// <summary>
        /// Opens the representing window and returns <see cref="Task"/> which completes
        /// when opened window is closed.
        /// </summary>
        /// <param name="parent">Object to be parent of the representing window.</param>
        public Task<object?> OpenAsync(object parent);

        /// <summary>
        /// Opens the representing window and returns <see cref="Task"/> which completes
        /// when opened window is closed.
        /// </summary>
        /// <param name="parentModule">Module to be parent of the representing window.</param>
        public Task<object?> OpenAsync(IBaseWindowModule parentModule);

        /// <summary>
        /// Closes the representing window if open.
        /// </summary>
        public void Close();

        /// <summary>
        /// Closes the representing window if open.
        /// </summary>
        /// <param name="result">
        /// Additional result data which can be retireved by awaiting <see cref="Task"/>
        /// returnd by one of the <c>OpenAsync()</c> methods.
        /// </param>
        public void Close(object? result);
    }

    /// <summary>
    /// Provides technology-agnostic abstraction over technology-specific window instance and its data context.
    /// </summary>
    /// <typeparam name="T">Type of module properties.</typeparam>
    public interface IBaseWindowModule<T> : IBaseWindowModule where T : class, new()
    {
        /// <summary>
        /// Provides additional data for module customization.
        /// </summary>
        public T Properties { get; set; }

        /// <inheritdoc/>
        IBaseWindowAware? IBaseWindowModule.DataContext => DataContext;

        /// <summary>
        /// Returns the data context of the representing window or <see langword="null"/> if no such was specified.
        /// </summary>
        new public IBaseWindowAware<T>? DataContext { get; }
    }
}
