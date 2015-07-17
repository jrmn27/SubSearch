﻿namespace SubSearch.WPF.View
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Input;

    using SubSearch.Data;

    using KeyEventArgs = System.Windows.Input.KeyEventArgs;

    /// <summary>Interaction logic for SelectionWindow.xaml</summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        /// <summary>The active window.</summary>
        private static MainWindow activeWindow;

        /// <summary>The selections.</summary>
        private readonly ObservableCollection<ItemData> selections = new ObservableCollection<ItemData>();

        /// <summary>The status.</summary>
        private string status;

        /// <summary>The title text.</summary>
        private string titleText;

        /// <summary>Initializes a new instance of the <see cref="MainWindow" /> class.</summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>The property changed.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Gets the selected item.</summary>
        public ItemData SelectedItem
        {
            get
            {
                return this.SelectionBox.SelectedItem as ItemData;
            }
        }

        /// <summary>Gets or sets the title text.</summary>
        public string TitleText
        {
            get
            {
                return this.titleText;
            }

            set
            {
                this.titleText = value;
                this.RaisePropertyChanged("TitleText");
                this.TitleBlock.Visibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>Gets or sets the status.</summary>
        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
                this.RaisePropertyChanged("Status");
            }
        }

        /// <summary>Gets the selections.</summary>
        public ObservableCollection<ItemData> Selections
        {
            get
            {
                return this.selections;
            }
        }

        /// <summary>
        /// The show progress.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        public static void ShowProgress(string title, string status)
        {
            CloseAll();
            activeWindow.SetProgress(title, status);
        }

        /// <summary>
        /// The get selection.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <returns>
        /// The <see cref="ItemData"/>.
        /// </returns>
        public static ItemData GetSelection(IEnumerable<ItemData> data, string title, string status)
        {
            CloseAll();
            activeWindow.SetSelections(data, title, status);
            var confirm = activeWindow.ShowDialog();
            if (confirm.HasValue && confirm.Value)
            {
                return activeWindow.SelectedItem;
            }

            return null;
        }

        /// <summary>The close all.</summary>
        public static void CloseAll()
        {
            if (activeWindow != null)
            {
                activeWindow.Close();
            }

            activeWindow = new MainWindow();
        }

        /// <summary>
        /// The list box item mouse double click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void ListBoxItemMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Accept();
        }

        /// <summary>
        /// The raise property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected void RaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// The set progress.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        private void SetProgress(string title, string status)
        {
            this.SelectionBox.Visibility = Visibility.Collapsed;
            this.ProgressBar.Visibility = Visibility.Visible;
            this.TitleText = title;
            this.Status = status;
            this.Show();
        }

        /// <summary>
        /// The set selections.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        private void SetSelections(IEnumerable<ItemData> data, string title, string status)
        {
            this.selections.Clear();
            foreach (var itemData in data)
            {
                this.selections.Add(itemData);
            }

            this.TitleText = title;
            this.Status = status;
            this.SelectionBox.Visibility = Visibility.Visible;
            this.ProgressBar.Visibility = Visibility.Collapsed;
        }

        /// <summary>The accept.</summary>
        private void Accept()
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// The list box item preview key up.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ListBoxItemPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Accept();
            }
        }

        /// <summary>
        /// The main window_ on preview key up.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MainWindow_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        /// <summary>
        /// The main window_ on content rendered.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MainWindow_OnContentRendered(object sender, EventArgs e)
        {
            this.SizeToContent = SizeToContent.Manual;
        }

        /// <summary>
        /// The main window_ on loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var mousePosition = Control.MousePosition;
            var activeScreenArea = Screen.FromPoint(mousePosition).WorkingArea;
            this.MaxHeight = activeScreenArea.Height;
            this.MaxWidth = activeScreenArea.Width;
            var left = mousePosition.X - (this.RenderSize.Width / 2);
            var top = mousePosition.Y - (this.RenderSize.Height / 2);
            this.Left = left > 0 ? left : 0;
            this.Top = top > 0 ? top : 0;
        }
    }
}