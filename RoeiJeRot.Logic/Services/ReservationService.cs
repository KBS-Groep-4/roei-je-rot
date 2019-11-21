using System;
using System.Collections.Generic;
using System.Text;
using RoeiJeRot.Database.Database;
using System.Linq;

namespace RoeiJeRot.Logic.Services
{
    public interface IReservationService
    {
        bool PlaceReservation(int boatType, int memberId, DateTime reservationDate, TimeSpan duration);
        void CancelBoatReservation(int reservationId);
    }

    class ReservationService : IReservationService
    {
        private readonly RoeiJeRotDbContext _context;

        public ReservationService(RoeiJeRotDbContext context)
        {
            _context = context;
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

        public List<SailingBoat> GetAvailableBoats(DateTime reservationDate, TimeSpan duration)
        {
            return null;
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
