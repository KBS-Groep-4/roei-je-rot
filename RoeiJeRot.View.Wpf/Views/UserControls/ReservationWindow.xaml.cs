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
    /// <summary>
    ///     Interaction logic for ReservationScreen.xaml
    /// </summary>
    public partial class ReservationScreen : CustomUserControl
    {
        private readonly IBoatService _boatService;
        private IReservationService _reservationService;
        private WindowManager _windowManager;
        private readonly IMailService _mailService;
        public event EventHandler<MessageArgs> StatusMessageUpdate;

        public ObservableCollection<ReservationViewModel> Items { get; set; } =
            new ObservableCollection<ReservationViewModel>();

        // Set data for the reservations view.
        public void SetReservationData()
        {
            var reservations = _reservationService.GetFutureReservations(_windowManager.UserSession.UserId)
                .Select(r => new ReservationViewModel
                {
                    Id = r.Id,
                    ReservationDate = r.Date,
                    Duration = r.Duration.ToString(@"hh\:mm"),
                    ReservedByUserId = r.ReservedBy.Username,
                    ReservedBoatId = r.ReservedSailingBoatId
                }).ToList();

            Items.Clear();
            foreach (var reservation in reservations) Items.Add(reservation);
        }

        public ReservationScreen(IBoatService boatService, IReservationService reservationService, IMailService mailService, WindowManager windowManager)
        {
            _boatService = boatService;
            _reservationService = reservationService;
            _mailService = mailService;
            _windowManager = windowManager;

            InitializeComponent();
            When.SelectedDate = DateTime.Today;

            UpdateAvailableList();

            SetReservationData();
            DeviceDataGrid.ItemsSource = Items;
        }

        public ObservableCollection<BoatTypeViewModel> ObservableAvailableTypes { get; set; }

        public void ReservButtonOnClick(object sender, RoutedEventArgs args)
        {
            TimeSpan time;
            if (TimeSpan.TryParse(Time.Text, out time))
            {
                if (time.TotalHours > 24)
                    return;

                int durationInt;
                if (int.TryParse(Duration.Text, out durationInt))
                {
                    var duration = TimeSpan.FromMinutes(durationInt);

                    var selectedItemObject = AvailableBoats.SelectedItem;
                    if (selectedItemObject == null)
                    {
                        StatusMessageUpdate?.Invoke(this, new MessageArgs("Reservering niet geplaatst: Geen boot geselecteerd.", Type.Red));
                        return;
                    }

                    var selectedType = (BoatTypeViewModel) selectedItemObject;

                    if (When.SelectedDate.HasValue)
                    {
                        bool result = _reservationService.PlaceReservation(selectedType.Id, _windowManager.UserSession.UserId, When.SelectedDate.Value + time,
                            duration).IsValid;
                        string message = _reservationService.PlaceReservation(selectedType.Id, _windowManager.UserSession.UserId, When.SelectedDate.Value + time,
                            duration).Reason;
                        if (result)
                        {
                            _mailService.SendConfirmation(_windowManager.UserSession.Email, _windowManager.UserSession.FirstName, When.SelectedDate.Value, duration);
                            StatusMessageUpdate?.Invoke(this, new MessageArgs("Reservering geplaatst.", Type.Green));
                            SetReservationData();
                        }
                        else
                        {
                            StatusMessageUpdate?.Invoke(this, new MessageArgs("Reservering niet geplaatst: " + message, Type.Red));
                        }

                        UpdateAvailableList();
                    }
                }
            }
        }

        public void OnReservationDetailChange(object sender, EventArgs args)
        {
            UpdateAvailableList();
        }

        public void UpdateAvailableList()
        {
            TimeSpan time;
            int durationInt;

            if (TimeSpan.TryParse(Time.Text, out time) && Duration != null &&
                int.TryParse(Duration.Text, out durationInt) &&
                When.SelectedDate.HasValue)
            {
                var boats = _reservationService.GetAvailableBoats(When.SelectedDate.Value + time,
                    TimeSpan.FromMinutes(durationInt));
                var availableTypes = new List<BoatType>();

                foreach (var boat in boats)
                {
                    var alreadyIn = false;
                    foreach (var type in availableTypes)
                        if (type.Id == boat.BoatTypeId)
                            alreadyIn = true;

                    if (!alreadyIn) availableTypes.Add(boat.BoatType);
                }

                ObservableAvailableTypes = new ObservableCollection<BoatTypeViewModel>(availableTypes
                    .Select(type => new BoatTypeViewModel
                    {
                        Id = type.Id,
                        Name = type.Name,
                        PossiblePassengers = type.PossiblePassengers,
                        RequiredLevel = type.RequiredLevel
                    })
                    .ToList());

                AvailableBoats.ItemsSource = ObservableAvailableTypes;
            }
            else if (AvailableBoats != null)
            {
                AvailableBoats.ItemsSource = null;
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            foreach (var data in DeviceDataGrid.SelectedItems)
            {
                _reservationService.CancelReservation(((ReservationViewModel)data).Id);
            }

            StatusMessageUpdate?.Invoke(this, new MessageArgs("Reservering(en) verwijderd.", Type.Green));
            SetReservationData();
        }
    }
}