using System;
using System.Collections.Generic;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic.Helper;
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

            List<BoatType> availableTypes = reservationService.AvailableBoatTypes(new DateTime(2019, 12, 26, 10, 00, 00), TimeSpan.FromMinutes(120));
            foreach (BoatType availableType in availableTypes)
            {
                Console.WriteLine(availableType.Name);
            }
        }
    }
}