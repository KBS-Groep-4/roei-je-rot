using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic.Services;
using RoeiJeRot.View.Wpf.Logic;
using RoeiJeRot.View.Wpf.ViewModels;
using RoeiJeRot.View.Wpf.Views.Windows;

namespace RoeiJeRot.View.Wpf.Views.UserControls
{
    public class MessageArgs : EventArgs
    {
        public string Message { get; set; }
        public string Error { get; set; }
        public MessageArgs(string message, string error)
        {
            Message = message;
            Error = error;
        }
    }
    /// <summary>
    ///     Interaction logic for ReservationOverviewScreen.xaml
    /// </summary>
    public partial class ReservationOverviewScreen : CustomUserControl
    {
        private readonly WindowManager _windowManager;
        private IMailService _mailService;
        private IReservationService _reservationService;
        public event EventHandler<MessageArgs> StatusMessageUpdate;

        public ReservationOverviewScreen(IReservationService reservationService, WindowManager windowManager, IMailService mailService)
        {
            _windowManager = windowManager;
            _mailService = mailService;
            InitializeComponent();
            SetReservationData(reservationService);
            DeviceDataGrid.ItemsSource = Items;
        }

        public ObservableCollection<ReservationViewModel> Items { get; set; } =
            new ObservableCollection<ReservationViewModel>();

        // Set data for the reservations view.
        public void SetReservationData(IReservationService reservationService)
        {
            _reservationService = reservationService;
            var reservations = reservationService.GetReservations()
                .Select(r => new ReservationViewModel
                {
                    Id = r.Id,
                    ReservationDate = r.Date,
                    Duration = r.Duration.ToString(@"hh\:mm"),
                    ReservedByUserId = r.ReservedBy.Username,
                    ReservedBoatId = r.ReservedSailingBoatId,
                    Email = r.ReservedBy.Email,
                    FirstName = r.ReservedBy.FirstName
                }).ToList();

            foreach (var reservation in reservations) Items.Add(reservation);
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            var toRemoveModel = new List<ReservationViewModel>();
            foreach (var data in DeviceDataGrid.SelectedItems)
            {
                var model = (ReservationViewModel)data;
                _mailService.SendCancelConfirmation(model.Email, model.FirstName, model.ReservationDate);
                _reservationService.CancelReservation((model).Id);
            }
            StatusMessageUpdate?.Invoke(this, new MessageArgs("Reservering(en) verwijderd.", "cancel"));
            toRemoveModel.ForEach(x => Items.Remove(x));
        }
    }
}