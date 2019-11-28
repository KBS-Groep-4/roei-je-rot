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
        private IBoatService _boatSerivce;
        private IReservationService _reservationService;
        public LoginWindow(IUserService userService, IBoatService boatService, IReservationService reservationService)
        {
            _loginLogic = new LoginLogic(userService);
            _boatSerivce = boatService;
            _reservationService = reservationService;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReserveerScherm rs = new ReserveerScherm();

            rs.Activate();
            rs.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OverzichtReserveringen rs = new OverzichtReserveringen(_boatSerivce, _reservationService);

            rs.Activate();
            rs.Show();
        }
    }
}
