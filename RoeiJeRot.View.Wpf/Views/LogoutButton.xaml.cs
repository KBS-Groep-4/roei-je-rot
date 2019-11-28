using System;
using System.Windows.Controls;

namespace RoeiJeRot.View.Wpf.Views
{
    /// <summary>
    ///     Interaction logic for LogoutButton.xaml
    /// </summary>
    public partial class LogoutButton : UserControl
    {
        private readonly LoginWindow _loginWindow;

        public LogoutButton()
        {
            _loginWindow = InstanceCreator.Instance.CreateInstance<LoginWindow>();
            InitializeComponent();
        }

        public void OnLogoutClick(object sender, EventArgs e)
        {
            _loginWindow.Activate();
            _loginWindow.Show();
        }
    }
}