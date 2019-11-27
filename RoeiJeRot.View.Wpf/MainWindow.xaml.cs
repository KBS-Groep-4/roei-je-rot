﻿using System.Windows;
using RoeiJeRot.Logic.Services;

namespace RoeiJeRot.View.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IUserService _userService;

        public MainWindow(IUserService userService)
        {
            _userService = userService;
            InitializeComponent();
        }

        private void OnReserveWindowClick(object sender, RoutedEventArgs e)
        {
            ReserveerScherm rs = new ReserveerScherm();

            rs.Activate();
            rs.Show();
        }

        private void OnLoginWindowClick(object sender, RoutedEventArgs e)
        {
            LoginWindow lw = new LoginWindow(_userService);

            lw.Activate();
            lw.Show();
        }

        private void btnGetUsers_Click(object sender, RoutedEventArgs e)
        {
            foreach (var user in _userService.GetUsers())
            {
                lstUserList.Items.Add(user.FirstName);
            }
        }

        private void LogoutButton_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
