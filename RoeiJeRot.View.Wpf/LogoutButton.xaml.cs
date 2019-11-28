using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RoeiJeRot.Logic.Services;

namespace RoeiJeRot.View.Wpf
{
    /// <summary>
    /// Interaction logic for LogoutButton.xaml
    /// </summary>
    public partial class LogoutButton : UserControl
    {
        private readonly LoginWindow _loginWindow;

        public LogoutButton()
        {
            _loginWindow = InstanceCreator.Instance.CreateInstance<LoginWindow>();
            InitializeComponent();
        }

        public void OnLogoutClick(Object sender, EventArgs e)
        {
            _loginWindow.Activate();
            _loginWindow.Show();
        }
    }
}
