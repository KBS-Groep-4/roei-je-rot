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
            List<SailingBoat> availableBoats = new List<SailingBoat>();

            foreach (SailingBoat boat in boats)
            {
                bool overlap = false;
                foreach (SailingReservation reservation in boat.SailingReservations)
                {
                    var startReservation = reservation.Date;
                    var endReservation = startReservation + TimeSpan.FromSeconds(reservation.Duration);

                    var startPlanned = reservationDate;
                    var endPlanned = startPlanned + duration;

                    if(startReservation < endPlanned && startPlanned < endReservation) overlap = true;
                }
                if (!overlap) availableBoats.Add(boat);
            }

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
