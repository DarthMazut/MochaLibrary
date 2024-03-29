﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Dialogs
{
    /// <summary>
    /// Marks implementing <see cref="IDialogModule"/> as able to perform <see cref="ShowAsync(object)"/> operation.
    /// <para>
    /// This interface allows for showing a <see cref="IDialogModule"/> object in non-modal manner.
    /// This possibility replaces <c>Windowing</c> namespace which <c>MochaLib</c> does not implement at this time.
    /// </para>
    /// </summary>
    public interface IDialogShow
    {
        /// <summary>
        /// Asynchronously opens a dialog represented by this instance in non-modal mode. 
        /// </summary>
        /// <param name="host">
        /// The object which hosts this dialog while being shown.
        /// <para>You should pass '<see langword="this"/>' here.</para>
        /// </param>
        Task ShowAsync(object host);
    }
}
