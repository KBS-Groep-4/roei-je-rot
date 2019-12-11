using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RoeiJeRot.Logic.Services;
using RoeiJeRot.View.Wpf.Logic;
using RoeiJeRot.View.Wpf.ViewModels;
using RoeiJeRot.View.Wpf.Views.Windows;

namespace RoeiJeRot.View.Wpf.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for ReservationOverviewScreen.xaml
    /// </summary>
    public partial class ReservationOverviewScreen : CustomUserControl
    {
        private readonly WindowManager _windowManager;

        public ReservationOverviewScreen(IReservationService reservationService, WindowManager windowManager)
        {
            _windowManager = windowManager;
            InitializeComponent();
            SetReservationData(reservationService);
            DeviceDataGrid.ItemsSource = Items;
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
                    ReservedByUserId = r.ReservedBy.Username,
                    ReservedBoatId = r.ReservedSailingBoatId
                }).ToList();

            foreach (var reservation in reservations) Items.Add(reservation);
        }

        public void OnClose()
        {
        }
    }
}