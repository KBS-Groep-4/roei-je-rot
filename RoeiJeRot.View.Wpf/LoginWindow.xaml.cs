using System;
using RoeiJeRot.Logic;
using RoeiJeRot.Logic.Services;
using System.Windows;

namespace RoeiJeRot.View.Wpf
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginLogic _loginLogic;

        public LoginWindow(IUserService userService)
        {
            _loginLogic = new LoginLogic(userService);
            InitializeComponent();
        }

        /// <summary>
        /// Called when clicked on "Inloggen" button. Checks if given input is correct.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnInloggenClick(object sender, RoutedEventArgs e)
        {
            if (_loginLogic.AuthenticateUser(UsernameTextbox.Text, PasswordTextbox.Password))
            {
                MessageBox.Show("Invoer incorrect.", "Invoer correct");
            }
            else
            {
                MessageBox.Show("Gebruikersnaam of wachtwoord incorrect.", "Invoer incorrect");
            }
        }
    }
}
