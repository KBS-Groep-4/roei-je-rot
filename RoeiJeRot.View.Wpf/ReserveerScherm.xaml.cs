using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore.Internal;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic.Services;

namespace RoeiJeRot.View.Wpf
{
    /// <summary>
    /// Interaction logic for ReserveerScherm.xaml
    /// </summary>
    public partial class ReserveerScherm : Window
    {
        public ReserveerScherm()
        {
            InitializeComponent();

            When.SelectedDate = DateTime.Today;

            RoeiJeRotDbContext context = new RoeiJeRotDbContext();

            IBoatService boatService = new BoatService(context);
            IReservationService reservationService = new ReservationService(context, boatService);

            List<SailingBoat> boats = reservationService.GetAvailableBoats(When.SelectedDate.Value, TimeSpan.FromHours(2), 1);
            List<BoatType> availableTypes = new List<BoatType>();

            foreach (var boat in boats)
            {
                availableTypes.Add(boat.BoatType);
            }

            availableTypes.Distinct((type, boatType) => type == boatType);

            AvailableBoats.ItemsSource = availableTypes;
        }
    }
}
