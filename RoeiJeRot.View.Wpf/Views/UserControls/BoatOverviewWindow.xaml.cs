using RoeiJeRot.Logic;
using RoeiJeRot.Logic.Services;
using RoeiJeRot.View.Wpf.Logic;
using RoeiJeRot.View.Wpf.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace RoeiJeRot.View.Wpf.Views.UserControls
{
    /// <summary>
    ///     Interaction logic for ReservationOverviewScreen.xaml
    /// </summary>
    public partial class BoatOverviewWindow : CustomUserControl
    {
        private readonly WindowManager _windowManager;
        private readonly IReservationService _reservationService;
        private readonly IBoatService _boatService;
        private readonly IMailService _mailService;

        public BoatOverviewWindow(IBoatService boatService, WindowManager windowManager, IReservationService reservationService, IMailService mailService)
        {
            _windowManager = windowManager;
            _reservationService = reservationService;
            _boatService = boatService;
            _mailService = mailService;
            InitializeComponent();
            SetBoatData(boatService);
            DeviceDataGrid.ItemsSource = Items;
        }

        public ObservableCollection<BoatTypeViewModel> Items { get; set; } =
            new ObservableCollection<BoatTypeViewModel>();

        // Set data for the boat view.
        public void SetBoatData(IBoatService boatService)
        {
            Items.Clear();
            var boats = boatService.GetAllBoats()
                .Select(r => new BoatTypeViewModel
                {
                    Id = r.Id,
                    PossiblePassengers = r.BoatType.PossiblePassengers,
                    RequiredLevel = r.BoatType.RequiredLevel,
                    Name = r.BoatType.Name,
                    HasCommanderSeat = r.BoatType.HasCommanderSeat.ToString(),
                    Status = r.Status.ToString()
                }).ToList();
            foreach (var boat in boats)
            {
                var status = (BoatState)Enum.Parse(typeof(BoatState), boat.Status);
                if (status == BoatState.InUse) boat.Status = "In gebruik";
                if (status == BoatState.InStock) boat.Status = "In magazijn";
                if (status == BoatState.InService) boat.Status = "Schade";
                if (boat.HasCommanderSeat.Equals("True")) boat.HasCommanderSeat = "Ja";
                if (boat.HasCommanderSeat.Equals("False")) boat.HasCommanderSeat = "Nee";
                Items.Add(boat);
            }
        }

        // Damage report button
        public void OnReportDamageClick(object sender, RoutedEventArgs args)
        {
            var selectedItemObject = DeviceDataGrid.SelectedItem;
            if (selectedItemObject == null)
            {
                MessageBox.Show("Geen boot geselecteerd");
                return;
            }

            var selectedType = (BoatTypeViewModel)selectedItemObject;

            if (selectedItemObject != null)
            {
                bool result = _boatService.ReportDamage(selectedType.Id, _windowManager.UserSession.UserId, DateTime.Now);

                if (result)
                {
                    _boatService.UpdateBoatStatus(selectedType.Id, BoatState.InService);
                    var listresult = _reservationService.AllocateBoatReservations(selectedType.Id);
                    foreach (var reservation in listresult)
                    {
                        _mailService.SendCancelMail(reservation.ReservedBy.Email, reservation.ReservedBy.FirstName, reservation.Date);
                    }
                    MessageBox.Show($"Schade gemeld, {listresult.Count()} reservering(en) konden niet omgezet worden. Er is een mail gestuurd aan deze leden");
                    SetBoatData(_boatService);
                }
                else MessageBox.Show("Schade niet gemeld");
            }
        }

        //Report damage free
        public void OnDisableDamageClick(object sender, RoutedEventArgs args)
        {
            var selectedItemObject = DeviceDataGrid.SelectedItem;
            if (selectedItemObject == null)
            {
                MessageBox.Show("Geen boot geselecteerd");
                return;
            }
            var selectedType = (BoatTypeViewModel)selectedItemObject;

            if (selectedItemObject != null)
            {
                bool result = _boatService.ReportDamage(selectedType.Id, _windowManager.UserSession.UserId, DateTime.Now);

                if (result)
                {
                    _boatService.UpdateBoatStatus(selectedType.Id, BoatState.InStock);
                    MessageBox.Show($"Boot {selectedType.Id} vrij gegeven");
                    SetBoatData(_boatService);
                }
                else MessageBox.Show("Schade niet afgemeld");
            }
        }
    }
}




