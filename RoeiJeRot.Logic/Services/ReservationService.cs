using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using RoeiJeRot.Database.Database;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace RoeiJeRot.Logic.Services
{
    public interface IReservationService
    {
        bool PlaceReservation(int boatType, int memberId, DateTime reservationDate, TimeSpan duration);
        void CancelBoatReservation(int reservationId);
    }

    public class ReservationService : IReservationService
    { 
        private readonly RoeiJeRotDbContext _context;
        private readonly IBoatService _boatService;

        public ReservationService(RoeiJeRotDbContext context, IBoatService boatService)
        {
            _context = context;
            _boatService = boatService;
        }

        /// <summary>
        /// Plaatst een reservatie voor een boot
        /// </summary>
        /// <param name="boatType">Type van de boot die je wilt reserveren</param>
        /// <param name="memberid">Id van het lid</param>
        /// <param name="reservationDate">Wanneer is de boot gereserveerd</param>
        /// <param name="duration">Hoelang duurt deze reservatie</param>
        /// <returns>Wanneer een reservatie is geplaatst true, als een reservatie niet geplaatst kan worden false</returns>
        public bool PlaceReservation(int boatType, int memberid, DateTime reservationDate, TimeSpan duration)
        {
            return false;
        }

        public List<SailingBoat> GetAvailableBoats(DateTime reservationDate, TimeSpan duration, int typeId)
        {
            List<SailingBoat> boats = _boatService.GetAllBoats(typeId);

            Console.WriteLine("--=All Boats=--");
            foreach (SailingBoat boat in boats)
                Console.WriteLine($"Boat {boat.Id} from type {boat.BoatTypeId}");

            List<SailingBoat> availableBoats = new List<SailingBoat>();
            foreach (SailingBoat boat in boats)
                if(boat.AvailableOn(reservationDate, duration)) availableBoats.Add(boat);

            Console.WriteLine("--=Available Boats=--");
            foreach (SailingBoat boat in availableBoats)
                Console.WriteLine($"Boat {boat.Id} from type {boat.BoatTypeId}");

            return availableBoats;
        }

        /// <summary>
        /// Get all sailingReservations
        /// </summary>
        /// <returns>All sailingReservations</returns>
        public List<SailingReservation> GetReservations()
        {
            return _context.Reservations.ToList();
        }

        public void CancelBoatReservation(int reservationId)
        {
            throw new NotImplementedException();
        }
    }
}
