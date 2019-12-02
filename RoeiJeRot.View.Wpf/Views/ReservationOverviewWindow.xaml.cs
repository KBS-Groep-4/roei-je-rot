using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RoeiJeRot.Logic.Services;
using RoeiJeRot.View.Wpf.ViewModels;

namespace RoeiJeRot.View.Wpf.Views
{
    /// <summary>
    ///     Interaction logic for ReservationOverviewWindow.xaml
    /// </summary>
    public partial class ReservationOverviewWindow : Window
    {
        public ReservationOverviewWindow(IReservationService reservationService)
        {
            InitializeComponent();
            SetReservationData(reservationService);
            DeviceDataGrid.ItemsSource = Items;
            btnLogout.OnClick += OnLogoutButtonClick;
        }

        private void OnLogoutButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var rs = InstanceCreator.Instance.CreateInstance<LoginWindow>();
            rs.Show();
        }

        public ObservableCollection<ReservationViewModel> Items { get; set; } =
            new ObservableCollection<ReservationViewModel>();

        // Set data for the reservations view.
        public void SetReservationData(IReservationService reservationService)
        {
            var reservations = reservationService.GetReservations()
                .Select(r => new ReservationViewModel
                {
                    Id = r.Id,
                    ReservationDate = r.Date.ToString("g"),
                    Duration = r.Duration.ToString(@"hh\:mm"),
                    ReservedByUserId = r.ReservedByUserId,
                    ReservedBoatId = r.ReservedSailingBoatId
                }).ToList();

            foreach (var reservation in reservations) Items.Add(reservation);
        }
    }
}