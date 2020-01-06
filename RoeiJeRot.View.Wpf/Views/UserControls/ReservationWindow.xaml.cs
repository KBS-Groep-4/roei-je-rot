using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic.Helper;
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
        private readonly IReservationService _reservationService;
        private readonly WindowManager _windowManager;
        public event EventHandler<MessageArgs> StatusMessageUpdate;

        public ObservableCollection<ReservationViewModel> Items { get; set; } =
            new ObservableCollection<ReservationViewModel>();

        private Dictionary<TimeSpan, List<BoatType>> TimeAvailableTypes = new Dictionary<TimeSpan, List<BoatType>>();

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
            _reservationService = reservationService;
            _windowManager = windowManager;

            InitializeComponent();
            When.SelectedDate = DateTime.Today;

            UpdateTimeAvailableTypes();

            SetReservationData();
            DeviceDataGrid.ItemsSource = Items;
        }

        public ObservableCollection<BoatTypeViewModel> ObservableAvailableTypes { get; set; }

        public void ReservButtonOnClick(object sender, RoutedEventArgs args)
        {
            if (int.TryParse(Duration.Text, out int durationInt))
            {
                TimeSpan duration = TimeSpan.FromMinutes(durationInt);

                if (When.SelectedDate.HasValue)
                {
                    if (AvailableTimes.SelectedItem != null && AvailableBoats.SelectedItem != null)
                    {
                        TimeSpan selectedTime = ((TimeViewModel)AvailableTimes.SelectedItem).Time;
                        BoatType selectedType = (BoatType)AvailableBoats.SelectedItem;

                        ReservationConstraintsMessage msg = _reservationService.PlaceReservation(selectedType.Id, _windowManager.UserSession.UserId,
                            When.SelectedDate.Value + selectedTime, duration);

                        StatusMessageUpdate?.Invoke(this, new MessageArgs(msg.Reason, msg.IsValid ? Type.Green : Type.Red));
                        return;
                    }
                }
            }
        }

        public void OnReservationDetailChange(object sender, EventArgs args)
        {
            UpdateTimeAvailableTypes();
        }

        public void OnBoatTypeChange(object sender, EventArgs args)
        {
            UpdateTimeList();
        }

        // Updates the time list
        private void UpdateTimeList()
        {
            if(AvailableTimes.SelectedItem == null)
            {
                ObservableCollection<TimeViewModel> times = new ObservableCollection<TimeViewModel>();
                foreach (TimeSpan availableTime in TimeAvailableTypes.Keys)
                {
                    if (AvailableBoats.SelectedItem == null)
                        times.Add(new TimeViewModel
                        {
                            Time = availableTime
                        });
                    else
                    {
                        BoatType selectedType = (BoatType)AvailableBoats.SelectedItem;
                        bool hasTypeAvailable = false;
                        foreach (BoatType type in TimeAvailableTypes[availableTime])
                        {
                            if (type.Id == selectedType.Id)
                                hasTypeAvailable = true;
                        }

                        if (hasTypeAvailable)
                            times.Add(new TimeViewModel
                            {
                                Time = availableTime
                            });
                    }
                }
                AvailableTimes.ItemsSource = times;
            } else if(AvailableBoats.SelectedItem == null)
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
                ObservableCollection<BoatType> typesObservableCollection = new ObservableCollection<BoatType>();
                if (AvailableTimes.SelectedItem == null)
                {
                    List<BoatType> types = new List<BoatType>();
                    foreach (List<BoatType> boatTypes in TimeAvailableTypes.Values)
                    {
                        foreach (BoatType boatType in boatTypes)
                        {
                            types.Add(boatType);
                        }
                    }

                    types = types.GroupBy(x => x.Id).Select(x => x.First()).ToList();

                    foreach (BoatType boatType in types)
                    {
                        typesObservableCollection.Add(boatType);
                    }
                }
                else
                {
                    TimeSpan selectedTime = ((TimeViewModel)AvailableTimes.SelectedItem).Time;

                    foreach (BoatType boatType in TimeAvailableTypes[selectedTime])
                    {
                        typesObservableCollection.Add(boatType);
                    }
                }
                AvailableBoats.ItemsSource = typesObservableCollection;
            } else if (AvailableTimes.SelectedItem == null)
            {
                UpdateTimeList();
            }
        }

        // Updates the dictionary
        public void UpdateTimeAvailableTypes()
        {
            TimeAvailableTypes = new Dictionary<TimeSpan, List<BoatType>>();
            if (When.SelectedDate.HasValue)
            {
                if (int.TryParse(Duration.Text, out int durationInt))
                {
                    TimeSpan duration = TimeSpan.FromMinutes(durationInt);
                    for (TimeSpan i = TimeSpan.Zero; i < new TimeSpan(0, 23, 59, 0); i += TimeSpan.FromMinutes(15))
                    {
                        if (DayChecker.IsDay(When.SelectedDate.Value + i, duration))
                        {
                            List<BoatType> availableTypes = _reservationService.AvailableBoatTypes(When.SelectedDate.Value + i, duration);
                            if (availableTypes.Any())
                            {
                                TimeAvailableTypes[i] = availableTypes;
                            }
                        }
                    }
                }
            }

            UpdateTimeList();
            UpdateBoatTypeList();
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