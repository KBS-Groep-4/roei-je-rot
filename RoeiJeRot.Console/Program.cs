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
            IReservationService reservationService = new ReservationService(context, boatService);

            List<SailingBoat> boats = boatService.GetAllBoats();

            foreach(SailingBoat boat in boats)
            {
                Console.WriteLine(boat.BoatType);
            }
        }
    }
}
