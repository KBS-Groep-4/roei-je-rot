using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RoeiJeRot.View.Wpf
{
    /// <summary>
    /// Interaction logic for OverzichtReservering.xaml
    /// </summary>
    public partial class OverzichtReserveringen : Window
    {
        public ObservableCollection<ReserveringOverzichtViewMod> Items { get; set; } = new ObservableCollection<ReserveringOverzichtViewMod>();

        public List<SailingBoat> BotenDatabase;

        public OverzichtReserveringen(IBoatService boatService, IReservationService reservationService)
        {
            InitializeComponent();
            //BotenDatabase = boatService.GetBoats();
            HaalDataOp(boatService, reservationService);
            DeviceDataGrid.ItemsSource = Items;
        }
        //Data uit de Database ophalen
        public void HaalDataOp(IBoatService boatService, IReservationService reservationService)
        {
            var reservationDB = reservationService.GetReservations().Select(date => new ReserveringOverzichtViewMod {
                Id = date.Id,
                ReservationDate = date.Date.ToString("g"),
                Duration = date.Duration.ToString(@"hh\:mm"),
                ReserverdByUserId = date.ReservedByUserId,
                ReserverdBoatId = date.ReservedSailingBoatId
            }).ToList();
            foreach (var item in reservationDB)
            {
                Items.Add(item);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Boten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Boden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    
        public class ReserveringOverzichtViewMod
        {
            public int Id { get; set; }
            public int BoatTypeName { get; set; }
            public int Status { get; set; }
            public bool Reserved { get; set; }
            public string ReservationDate { get; set; }
            public string Duration { get; set; }
            public int ReserverdByUserId { get; set; }
            public int ReserverdBoatId { get; set; }
                
        }
    }    
}

