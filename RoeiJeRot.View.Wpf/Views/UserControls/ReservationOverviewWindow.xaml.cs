using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using RoeiJeRot.Logic.Services;
using RoeiJeRot.View.Wpf.Logic;
using RoeiJeRot.View.Wpf.ViewModels;

namespace RoeiJeRot.View.Wpf.Views.UserControls
{
    public enum Type
    {
        Red = 0,
        Green = 1
    }

    public class MessageArgs : EventArgs
    {
        public MessageArgs(string message, Type type)
        {
            Message = message;
            Type = type;
        }

        public string Message { get; }
        public Type Type { get; }
    }

    /// <summary>
    ///     Interaction logic for ReservationOverviewScreen.xaml
    /// </summary>
    public partial class ReservationOverviewScreen : CustomUserControl
    {
        private readonly WindowManager _windowManager;
        private readonly IMailService _mailService;
        private IReservationService _reservationService;

        public ReservationOverviewScreen(IReservationService reservationService, WindowManager windowManager,
            IMailService mailService)
        {
            _windowManager = windowManager;
            _mailService = mailService;
            InitializeComponent();
            SetReservationData(reservationService);
            DeviceDataGrid.ItemsSource = Items;
        }

        public ObservableCollection<ReservationViewModel> Items { get; set; } =
            new ObservableCollection<ReservationViewModel>();

        public event EventHandler<MessageArgs> StatusMessageUpdate;

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
                var model = (ReservationViewModel) data;
                _mailService.SendCancelConfirmation(model.Email, model.FirstName, model.ReservationDate);
                _reservationService.CancelReservation(model.Id);
                toRemoveModel.Add(model);
            }

            StatusMessageUpdate?.Invoke(this, new MessageArgs("Reservering(en) verwijderd.", Type.Green));
            toRemoveModel.ForEach(x => Items.Remove(x));
        }
    }
}