using System;
using System.Collections.Generic;
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

            Console.WriteLine("--== Seeding process=--");
            for (int i = 0; i < 5; i++)
            {
                bool result = reservationService.PlaceReservation(1, 1, DateTime.Now + TimeSpan.FromDays(9), TimeSpan.FromMinutes(90));

                Console.WriteLine($"Reservation is" + (result ? "" : " not") + " placed");
            }

            Console.WriteLine("--== Reallocation process ==--");
            List<SailingReservation> notPlaced = reservationService.AllocateBoatReservations(1);

            foreach (SailingReservation reservation in notPlaced)
                Console.WriteLine(reservation.Id + " could not be replaced");
        }
    }
}