using System.Windows;
using System.Windows.Input;
using RoeiJeRot.Logic.Services;
using RoeiJeRot.View.Wpf.Logic;
using Window = System.Windows.Window;

namespace RoeiJeRot.View.Wpf.Views.Windows
{
    /// <summary>
    ///     Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly WindowManager _windowManager;

        public LoginWindow(WindowManager windowManager)
        {
            _windowManager = windowManager;
            InitializeComponent();
            this.headerBar.BtnCloseClick += CloseClick;
            this.headerBar.BtnMinClick += MinimizeClick;
            this.headerBar.BtnMaxClick += MaximizeRestoreClick;

            headerBar.logoutButton.Visibility = Visibility.Hidden;
        }

        /// <summary>
        ///     Called when clicked on "Inloggen" button. Checks if given input is correct.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void OnLogInClick(object sender, RoutedEventArgs e)
        {
            if (!_windowManager.ShowMainWindow(UsernameTextbox.Text, PasswordTextbox.Password))
                MessageBox.Show("Gebruikersnaam of wachtwoord incorrect.", "Invoer incorrect");
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            _windowManager.CurrentWindow.Close();
        }

        private void MaximizeRestoreClick(object sender, RoutedEventArgs e)
        {
            _windowManager.CurrentWindow.MaximizeWindow();
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            _windowManager.CurrentWindow.MinimizeWindow();
        }

        private void OnEnterClick(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnLogInClick(this, e);
            }
        }
    }
}