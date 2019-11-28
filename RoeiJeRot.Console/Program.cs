using System;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic.Services;

namespace RoeiJeRot.View.CustomConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var context = new RoeiJeRotDbContext();

            IBoatService boatService = new BoatService(context);
            var reservationService = new ReservationService(context, boatService);

            var placed =
                reservationService.PlaceReservation(4, 7, new DateTime(2020, 4, 9, 13, 20, 0),
                    TimeSpan.FromMinutes(90));

            if (placed) Console.WriteLine("Geplaatst");
            else Console.WriteLine("Geen bestelling gelpaatst");
        }
    }
}