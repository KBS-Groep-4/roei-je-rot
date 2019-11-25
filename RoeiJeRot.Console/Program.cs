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
            ReservationService   reservationService = new ReservationService(context, boatService);

            List<SailingBoat> boats = boatService.GetAllBoats(5);
            List<SailingBoat> avail = reservationService.GetAvailableBoats(new DateTime(2020, 4, 9, 13, 15, 0), TimeSpan.FromMinutes(120), 5);

            foreach(SailingBoat boat in avail)
            {
                Console.WriteLine(boat.Id);
            }
        }
    }
}
