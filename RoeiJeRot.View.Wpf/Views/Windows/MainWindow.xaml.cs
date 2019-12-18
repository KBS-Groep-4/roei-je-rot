using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic;
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

        private Dictionary<PermissionType, Button> Buttons { get; set; }

        public MainWindow(WindowManager windowManager)
        {
            _windowManager = windowManager;
            InitializeComponent();
            this.headerBar.BtnCloseClick += CloseClick;
            this.headerBar.BtnMinClick += MinimizeClick;
            this.headerBar.BtnMaxClick += MaximizeRestoreClick;
            this.headerBar.LogoutClick += OnLogout;
            
            InitializeButtons();
            LoadButtonsForUser();
        }

        private void InitializeButtons()
        {
            Buttons = new Dictionary<PermissionType, Button>();
            var reservationOverViewWindow = new Button() { Content = "Reservering Overzicht", };
            reservationOverViewWindow.Click += OnReservationOverviewClick;

            var reservationWindow = new Button() {Content = "Reservering Plaatsen",};
            reservationWindow.Click += OnReservationClick;

            Buttons.Add(PermissionType.Admin | PermissionType.Staff, reservationOverViewWindow);
            Buttons.Add(PermissionType.Admin | PermissionType.Mc | PermissionType.Member | PermissionType.Staff | PermissionType.Wc, reservationWindow);
        }

        private void LoadButtonsForUser()
        {
            var permissionType = _windowManager.UserSession.PermissionType;

            var buttonsForUser = Buttons
                .Where(x => x.Key.HasFlag(permissionType ))
                .Select(x => x.Value);

            foreach (var buttonForUser in buttonsForUser)
            {
                pnlPageButtons.Children.Add(buttonForUser);
            }
        }

        private void OnReservationOverviewClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button)
            {
                ReservationOverviewScreen ros = InstanceCreator.Instance.CreateInstance<ReservationOverviewScreen>();
                ros.StatusMessageUpdate += OnStatusMessageUpdate;
                _windowManager.CurrentWindow.PushEmbeddedScreen(ros);
            }

            OnScreenUpdate();
        }

        private void OnReservationClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button)
            {
                ReservationScreen rs = InstanceCreator.Instance.CreateInstance<ReservationScreen>();
                rs.StatusMessageUpdate += OnStatusMessageUpdate;
                _windowManager.CurrentWindow.PushEmbeddedScreen(rs);
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

        private void OnStatusMessageUpdate(Object sender, MessageArgs args)
        {
            StatusLabel.Content = args.Message;
            switch (args.Error)
            {
                case "error":
                    StatusLabel.Background = Brushes.Red;
                    StatusLabel.BorderBrush = Brushes.Red;
                    break;
                case "succeed":
                    StatusLabel.Background = Brushes.LimeGreen;
                    StatusLabel.BorderBrush = Brushes.LimeGreen;
                    break;
                case "cancel":
                    StatusLabel.Background = Brushes.LimeGreen;
                    StatusLabel.BorderBrush = Brushes.LimeGreen;
                    break;
            }
            OnScreenUpdate();
        }

        private void OnScreenUpdate()
        {
            screenGrid.Children.Clear();
            var screen = _windowManager.CurrentWindow.TopScreen();
            screenGrid.Children.Add(screen);
            screen.HorizontalAlignment = HorizontalAlignment.Left;
            screen.VerticalAlignment = VerticalAlignment.Top;

            Grid.SetRow(screen, 1);
            Grid.SetColumn(screen, 1);
        }
    }
}
