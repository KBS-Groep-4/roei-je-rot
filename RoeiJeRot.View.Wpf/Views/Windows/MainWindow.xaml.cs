using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RoeiJeRot.View.Wpf.Logic;
using RoeiJeRot.View.Wpf.ViewModels;
using RoeiJeRot.View.Wpf.Views.UserControls;
using Window = System.Windows.Window;

namespace RoeiJeRot.View.Wpf.Views.Windows
{
    /// <summary>
    /// Interaction logic for ClieMainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WindowManager _windowManager;

        public MainWindow(WindowManager windowManager)
        {
            _windowManager = windowManager;
            InitializeComponent();
            this.headerBar.BtnCloseClick += CloseClick;
            this.headerBar.BtnMinClick += MinimizeClick;
            this.headerBar.BtnMaxClick += MaximizeRestoreClick;
            this.headerBar.LogoutClick += OnLogout;

            LoadButtons();
        }

        private void LoadButtons()
        {
            var reservationOverViewWindow = new Button() {Content = "Reservering Overzicht",};
            reservationOverViewWindow.Click += OnReservationOverviewClick;
            pnlPageButtons.Children.Add(reservationOverViewWindow);

            var reservationWindow = new Button() { Content = "Reservering Plaatsen", };
            reservationWindow.Click += OnReservationClick;
            pnlPageButtons.Children.Add(reservationWindow);
        }

        private void OnReservationOverviewClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button)
            {
                _windowManager.CurrentWindow.PushEmbeddedScreen(InstanceCreator.Instance
                    .CreateInstance<ReservationOverviewScreen>());
            }

            OnScreenUpdate();
        }

        private void OnReservationClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button)
            {
                _windowManager.CurrentWindow.PushEmbeddedScreen(InstanceCreator.Instance
                    .CreateInstance<ReservationScreen>());
            }

            OnScreenUpdate();
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

        private void OnLogout(object sender, RoutedEventArgs e)
        {
            _windowManager.Logout();
        }

        private void OnScreenUpdate()
        {
            var screen = _windowManager.CurrentWindow.TopScreen();
            screenGrid.Children.Add(screen);
            screen.HorizontalAlignment = HorizontalAlignment.Stretch;
            screen.VerticalAlignment = VerticalAlignment.Top;

            Grid.SetRow(screen, 1);
            Grid.SetColumn(screen, 1);
        }
    }
}
