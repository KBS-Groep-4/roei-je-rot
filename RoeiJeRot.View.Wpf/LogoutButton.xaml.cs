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
        private readonly IUserService _userService;

        public LogoutButton()
        {
            _userService = InstanceCreator.Instance.CreateInstance<IUserService>();
            InitializeComponent();
        }

        public void OnLogoutClick(Object sender, EventArgs e)
        {
            LoginWindow lw = new LoginWindow(_userService);

            lw.Activate();
            lw.Show();
        }
    }
}
