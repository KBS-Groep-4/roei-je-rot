using System;
using System.Windows;
using System.Windows.Controls;

namespace RoeiJeRot.View.Wpf.Views.Components
{
    /// <summary>
    ///     Interaction logic for LogoutButton.xaml
    /// </summary>
    public partial class LogoutButton : UserControl
    {
        public LogoutButton()
        {
            InitializeComponent();
        }

        public event EventHandler<RoutedEventArgs> Click;

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}