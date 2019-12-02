using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RoeiJeRot.View.Wpf.Views
{
    /// <summary>
    ///     Interaction logic for LogoutButton.xaml
    /// </summary>
    public partial class LogoutButton : UserControl
    {
        private readonly LoginWindow _loginWindow;

        public event EventHandler<RoutedEventArgs> OnClick;

        public LogoutButton()
        {
            _loginWindow = InstanceCreator.Instance.CreateInstance<LoginWindow>();
            InitializeComponent();
        }
        
        private void LogOutButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnClick?.Invoke(this, e);
        }
    }
}