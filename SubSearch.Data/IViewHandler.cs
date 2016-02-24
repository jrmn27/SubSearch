﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewHandler.cs" company="">
//   
// </copyright>
// <summary>
//   The delegate for view custom action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SubSearch.Data
{
    using System;
    using System.Collections.Generic;

    /// <summary>The delegate for view custom action.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="parameter">The action parameter.</param>
    /// <param name="actionNames">The action names.</param>
    public delegate void CustomActionDelegate(IViewHandler sender, object parameter, params string[] actionNames);

    /// <summary>The ViewHandler interface.</summary>
    public interface IViewHandler : IDisposable
    {
        /// <summary>Occurs when the view handler custom action is requested.</summary>
        event CustomActionDelegate CustomActionRequested;

        /// <summary>Occurs when the view handler is disposed.</summary>
        event Action Disposed;

        /// <summary>Gets or sets the target file.</summary>
        string TargetFile { get; set; }

        /// <summary>The get selection.</summary>
        /// <param name="data">The data.</param>
        /// <param name="title">The title.</param>
        /// <param name="status">The status.</param>
        /// <returns>The <see cref="ItemData"/>.</returns>
        ItemData GetSelection(ICollection<ItemData> data, string title, string status);

        /// <summary>Notifies a message.</summary>
        /// <param name="message">The message.</param>
        void Notify(string message);

        /// <summary>The show progress.</summary>
        /// <param name="title">The title.</param>
        /// <param name="status">The status.</param>
        void ShowProgress(string title, string status);

        /// <summary>Sets the progress.</summary>
        /// <param name="done">Done.</param>
        /// <param name="total">Total</param>
        void ShowProgress(int done, int total);

        /// <summary>Starts the view handler.</summary>
        void Start();

        /// <summary>Continues the pending operation and cancel any selection.</summary>
        void Continue();
    }
}