using System.Windows;
using RoeiJeRot.Logic.Services;

namespace RoeiJeRot.View.Wpf.Views
{
    /// <summary>
    ///     Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginWindow(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            InitializeComponent();
        }

        /// <summary>
        ///     Called when clicked on "Inloggen" button. Checks if given input is correct.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnInloggenClick(object sender, RoutedEventArgs e)
        {
            if (_authenticationService.AuthenticateUser(UsernameTextbox.Text, PasswordTextbox.Password))
                ShowReservationOverviewWindow();
            else
                MessageBox.Show("Gebruikersnaam of wachtwoord incorrect.", "Invoer incorrect");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rs = InstanceCreator.Instance.CreateInstance<ReservationWindow>();
            rs.Activate();
            rs.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ShowReservationOverviewWindow();
        }

        private void ShowReservationOverviewWindow()
        {
            var rs = InstanceCreator.Instance.CreateInstance<ReservationOverviewWindow>();

            rs.Activate();
            rs.Show();
        }
    }
}