using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic.Services;

namespace RoeiJeRot.View.Wpf
{
    public class BoatTypeViewModel
    {
        public int Id { get; set; }
        public int PossiblePassengers { get; set; }
        public int RequiredLevel { get; set; }
        public string Name { get; set; }
        protected bool HasCommanderSeat { get; set; }
    }

    /// <summary>
    /// Interaction logic for ReserveerScherm.xaml
    /// </summary>
    public partial class ReserveerScherm : Window
    {
        public ObservableCollection<BoatTypeViewModel> ObservableAvailableTypes { get; set; }

        private RoeiJeRotDbContext context;
        private IBoatService boatService;
        private IReservationService reservationService;

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
                    TimeSpan duration = TimeSpan.FromMinutes(durationInt);

                    Object selectedItemObject = AvailableBoats.SelectedItem;
                    if (selectedItemObject == null)
                    {
                        MessageBox.Show("Geen boot geselecteerd :(");
                        return;
                    }

                    BoatTypeViewModel selectedType = (BoatTypeViewModel)selectedItemObject;

                    if (When.SelectedDate.HasValue)
                    {
                        reservationService.PlaceReservation(selectedType.Id, 3, When.SelectedDate.Value + time, duration);
                        MessageBox.Show("Reservering geplaatst :D");
                        UpdateAvailableList();
                    }
                }
            }
        }

        public void OnReservationDetailChange(Object sender, EventArgs args)
        {
            UpdateAvailableList();
        }

        public void UpdateAvailableList()
        {
            TimeSpan time;
            int durationInt;
            if (TimeSpan.TryParse(Time.Text, out time) && Duration != null && int.TryParse(Duration.Text, out durationInt) &&
                When.SelectedDate.HasValue)
            {
                List<SailingBoat> boats = reservationService.GetAvailableBoats(When.SelectedDate.Value + time,
                    TimeSpan.FromMinutes(durationInt));
                List<BoatType> availableTypes = new List<BoatType>();

                foreach (var boat in boats)
                {
                    bool alreadyIn = false;
                    foreach (var type in availableTypes)
                    {
                        if (type.Id == boat.BoatTypeId)
                            alreadyIn = true;
                    }

                    if (!alreadyIn) availableTypes.Add(boat.BoatType);
                }

                ObservableAvailableTypes = new ObservableCollection<BoatTypeViewModel>(availableTypes
                    .Select(type => new BoatTypeViewModel()
                    {
                        Id = type.Id,
                        Name = type.Name,
                        PossiblePassengers = type.PossiblePassengers,
                        RequiredLevel = type.RequiredLevel
                    })
                    .ToList());

                AvailableBoats.ItemsSource = ObservableAvailableTypes;
            }
            else if(AvailableBoats != null) AvailableBoats.ItemsSource = null;
        }

        public ReserveerScherm()
        { 
            context = new RoeiJeRotDbContext();
            boatService = new BoatService(context);
            reservationService = new ReservationService(context, boatService);

            InitializeComponent();

            When.SelectedDate = DateTime.Today;

            UpdateAvailableList();
        }
    }
}
