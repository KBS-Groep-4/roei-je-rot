using System;
using RoeiJeRot.Logic.Services;
using RoeiJeRot.Database;
using RoeiJeRot.Database.Database;
using System.Collections.Generic;

namespace RoeiJeRot.View.CustomConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RoeiJeRotDbContext context = new RoeiJeRotDbContext();

            IBoatService boatService = new BoatService(context);
            ReservationService reservationService = new ReservationService(context, boatService);

            bool placed = reservationService.PlaceReservation(4, 7, new DateTime(2020, 4, 9, 13, 20, 0), TimeSpan.FromMinutes(90));

            if (placed) Console.WriteLine("Geplaatst");
            else Console.WriteLine("Geen bestelling gelpaatst");
        }
    }
}
