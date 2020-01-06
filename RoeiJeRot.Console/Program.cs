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
            IReservationService reservationService = new ReservationService(context, boatService);

            var availableTypes =
                reservationService.AvailableBoatTypes(new DateTime(2019, 12, 26, 10, 00, 00),
                    TimeSpan.FromMinutes(120));
            foreach (var availableType in availableTypes) Console.WriteLine(availableType.Name);
        }
    }
}