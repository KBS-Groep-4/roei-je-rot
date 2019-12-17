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
        private readonly IReservationService _reservationService;
        private readonly IMailService _mailService;
        private readonly WindowManager _windowManager;

        public ReservationScreen(IBoatService boatService, IReservationService reservationService, IMailService mailService, WindowManager windowManager)
        {
            _boatService = boatService;
            _reservationService = reservationService;
            _mailService = mailService;
            _windowManager = windowManager;
            InitializeComponent();
            When.SelectedDate = DateTime.Today;
            
            UpdateAvailableList();
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
                        MessageBox.Show("Geen boot geselecteerd");
                        return;
                    }

                    var selectedType = (BoatTypeViewModel) selectedItemObject;

                    if (When.SelectedDate.HasValue)
                    {
                        bool result = _reservationService.PlaceReservation(selectedType.Id, _windowManager.UserSession.UserId, When.SelectedDate.Value + time,
                            duration);
                        if (result)
                        {
                            _mailService.SendConfirmation(_windowManager.UserSession.Email, _windowManager.UserSession.FirstName, When.SelectedDate.Value + time, duration);
                            MessageBox.Show("Reservering geplaatst");
                        }
                        else MessageBox.Show("Reservatie niet geplaatst");

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
                var boats = _boatService.GetAvailableBoats(When.SelectedDate.Value + time,
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

        public void OnClose()
        {
        }
    }
}