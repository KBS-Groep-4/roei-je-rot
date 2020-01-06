using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic.Helper;
using RoeiJeRot.Logic.Services;
using RoeiJeRot.View.Wpf.Logic;
using RoeiJeRot.View.Wpf.ViewModels;

namespace RoeiJeRot.View.Wpf.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for ReservationScreen.xaml
    /// </summary>
    public partial class ReservationScreen : CustomUserControl
    {
        private readonly IReservationService _reservationService;
        private readonly WindowManager _windowManager;

        private Dictionary<TimeSpan, List<BoatType>> TimeAvailableTypes = new Dictionary<TimeSpan, List<BoatType>>();

        public ReservationScreen(IBoatService boatService, IReservationService reservationService,
            IMailService mailService, WindowManager windowManager)
        {
            _reservationService = reservationService;
            _windowManager = windowManager;

            InitializeComponent();
            When.SelectedDate = DateTime.Today;

            UpdateDictionary();

            SetReservationData();
            DeviceDataGrid.ItemsSource = Items;
        }

        public ObservableCollection<ReservationViewModel> Items { get; set; } =
            new ObservableCollection<ReservationViewModel>();

        public ObservableCollection<BoatTypeViewModel> ObservableAvailableTypes { get; set; }
        public event EventHandler<MessageArgs> StatusMessageUpdate;

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

        public void ReservButtonOnClick(object sender, RoutedEventArgs args)
        {
            if (int.TryParse(Duration.Text, out var durationInt))
            {
                var duration = TimeSpan.FromMinutes(durationInt);

                if (When.SelectedDate.HasValue)
                    if (AvailableTimes.SelectedItem != null && AvailableBoats.SelectedItem != null)
                    {
                        var selectedTime = ((TimeViewModel) AvailableTimes.SelectedItem).Time;
                        var selectedType = (BoatType) AvailableBoats.SelectedItem;

                        var msg = _reservationService.PlaceReservation(selectedType.Id,
                            _windowManager.UserSession.UserId,
                            When.SelectedDate.Value + selectedTime, duration);

                        StatusMessageUpdate?.Invoke(this,
                            new MessageArgs(msg.Reason, msg.IsValid ? Type.Green : Type.Red));

                        if (msg.IsValid)
                        {
                            UpdateDictionary();
                            SetReservationData();
                        }
                    }
            }
        }

        public void OnReservationDetailChange(object sender, EventArgs args)
        {
            UpdateDictionary();
        }

        public void OnBoatTypeChange(object sender, EventArgs args)
        {
            UpdateTimeList();
        }

        // Updates the time list
        private void UpdateTimeList()
        {
            if (AvailableTimes.SelectedItem == null)
            {
                var times = new ObservableCollection<TimeViewModel>();
                foreach (var availableTime in TimeAvailableTypes.Keys)
                    if (AvailableBoats.SelectedItem == null)
                    {
                        times.Add(new TimeViewModel
                        {
                            Time = availableTime
                        });
                    }
                    else
                    {
                        var selectedType = (BoatType) AvailableBoats.SelectedItem;
                        var hasTypeAvailable = false;
                        foreach (var type in TimeAvailableTypes[availableTime])
                            if (type.Id == selectedType.Id)
                                hasTypeAvailable = true;

                        if (hasTypeAvailable)
                            times.Add(new TimeViewModel
                            {
                                Time = availableTime
                            });
                    }

                AvailableTimes.ItemsSource = times;
            }
            else if (AvailableBoats.SelectedItem == null)
            {
                UpdateBoatTypeList();
            }
        }

        public void OnTimeChange(object sender, EventArgs args)
        {
            UpdateBoatTypeList();
        }

        // Update the boat type list
        private void UpdateBoatTypeList()
        {
            if (AvailableBoats.SelectedItem == null)
            {
                var typesObservableCollection = new ObservableCollection<BoatType>();
                if (AvailableTimes.SelectedItem == null)
                {
                    var types = new List<BoatType>();
                    foreach (var boatTypes in TimeAvailableTypes.Values)
                    foreach (var boatType in boatTypes)
                        types.Add(boatType);

                    types = types.GroupBy(x => x.Id).Select(x => x.First()).ToList();

                    foreach (var boatType in types) typesObservableCollection.Add(boatType);
                }
                else
                {
                    var selectedTime = ((TimeViewModel) AvailableTimes.SelectedItem).Time;

                    foreach (var boatType in TimeAvailableTypes[selectedTime]) typesObservableCollection.Add(boatType);
                }

                AvailableBoats.ItemsSource = typesObservableCollection;
            }
            else if (AvailableTimes.SelectedItem == null)
            {
                UpdateTimeList();
            }
        }

        // Updates the dictionary
        public void UpdateDictionary()
        {
            TimeAvailableTypes = new Dictionary<TimeSpan, List<BoatType>>();
            if (When.SelectedDate.HasValue)
                if (int.TryParse(Duration.Text, out var durationInt))
                {
                    var duration = TimeSpan.FromMinutes(durationInt);
                    for (var i = TimeSpan.Zero; i < new TimeSpan(0, 23, 59, 0); i += TimeSpan.FromMinutes(15))
                        if (ReservationConstraints.IsValid(When.SelectedDate.Value + i, duration, _reservationService, _windowManager.UserSession.UserId).IsValid)
                        {
                            var availableTypes =
                                _reservationService.AvailableBoatTypes(When.SelectedDate.Value + i, duration);
                            if (availableTypes.Any()) TimeAvailableTypes[i] = availableTypes;
                        }
                }

            UpdateTimeList();
            UpdateBoatTypeList();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            foreach (var data in DeviceDataGrid.SelectedItems)
                _reservationService.CancelReservation(((ReservationViewModel) data).Id);

            StatusMessageUpdate?.Invoke(this, new MessageArgs("Reservering(en) verwijderd.", Type.Green));
            SetReservationData();
        }
    }
}