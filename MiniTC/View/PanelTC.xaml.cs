using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MiniTC.View
{
    /// <summary>
    /// Logika interakcji dla klasy PanelTC.xaml
    /// </summary>
    public partial class PanelTC : UserControl
    {
        #region Properites
        public string Path
        {
            get { return (string)GetValue(PathDP); }
            set { SetValue(PathDP, value); }
        }

        public static readonly DependencyProperty PathDP = DependencyProperty.Register(
           nameof(Path), typeof(string), typeof(PanelTC), new FrameworkPropertyMetadata(null));

        public string[] DrivesList
        {
            get { return (string[])GetValue(DrivesListDP); }
            set { SetValue(DrivesListDP, value); }
        }

        public static readonly DependencyProperty DrivesListDP = DependencyProperty.Register(
           nameof(DrivesList), typeof(string[]), typeof(PanelTC), new FrameworkPropertyMetadata(null));

        public List<string> FileList
        {
            get { return (List<string>)GetValue(FileListDP); }
            set { SetValue(FileListDP, value); }
        }

        public static readonly DependencyProperty FileListDP = DependencyProperty.Register(
           nameof(FileList), typeof(List<string>), typeof(PanelTC), new FrameworkPropertyMetadata(null));

        public int SelectedDrive
        {
            get { return (int)GetValue(SelectedDriveDP); }
            set { SetValue(SelectedDriveDP, value); }
        }

        public static readonly DependencyProperty SelectedDriveDP = DependencyProperty.Register(
           nameof(SelectedDrive), typeof(int), typeof(PanelTC), new FrameworkPropertyMetadata(null));

        public int CurrentItem
        {
            get { return (int)GetValue(CurrentItemDP); }
            set { SetValue(CurrentItemDP, value); }
        }

        public static readonly DependencyProperty CurrentItemDP = DependencyProperty.Register(
           nameof(CurrentItem), typeof(int), typeof(PanelTC), new FrameworkPropertyMetadata(null));
        #endregion

        #region Events

        public static readonly RoutedEvent DriveChangedRegistered =
        EventManager.RegisterRoutedEvent(nameof(DriveChanged),
                     RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                     typeof(PanelTC));

        public event RoutedEventHandler DriveChanged
        {
            add { AddHandler(DriveChangedRegistered, value); }
            remove { RemoveHandler(DriveChangedRegistered, value); }
        }

        void RaiseDriveChanged()
        {
            RoutedEventArgs newEventArgs =
                    new RoutedEventArgs(PanelTC.DriveChangedRegistered);
            RaiseEvent(newEventArgs);
        }

        public static readonly RoutedEvent DirectoryChangedRegistered =
        EventManager.RegisterRoutedEvent(nameof(DirectoryChanged),
                     RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                     typeof(PanelTC));

        public event RoutedEventHandler DirectoryChanged
        {
            add { AddHandler(DirectoryChangedRegistered, value); }
            remove { RemoveHandler(DirectoryChangedRegistered, value); }
        }

        void RaiseDirectoryChanged()
        {
            RoutedEventArgs newEventArgs =
                    new RoutedEventArgs(PanelTC.DirectoryChangedRegistered);
            RaiseEvent(newEventArgs);
        }
        #endregion

        public PanelTC()
        {
            InitializeComponent();
        }

        private void DrivesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseDriveChanged();
        }

        private void FilesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseDirectoryChanged();
        }
    }
}
