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
        List<SailingBoat> GetAvailableBoats(DateTime reservationDate, TimeSpan duration);
        List<SailingBoat> GetAvailableBoats(DateTime reservationDate, TimeSpan duration, int typeId);
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
            var availableBoats = GetAvailableBoats(reservationDate, duration, boatType);

            if (availableBoats.Count > 0)
            {
                SailingBoat boatToReserve = null;

                int min = int.MaxValue;
                foreach (var boat in availableBoats)
                    if (boat.SailingReservations.Count < min) boatToReserve = boat;

                _context.Reservations.Add(new SailingReservation()
                {
                    Date = reservationDate,
                    Duration = duration,
                    ReservedByUserId = memberid,
                    ReservedSailingBoatId = boatToReserve.Id
                });

                _context.SaveChanges();
                return true;
            }
            else return false;
        }

        public List<SailingBoat> GetAvailableBoats(DateTime reservationDate, TimeSpan duration)
        {
            var boats = _boatService.GetAllBoats();
            List<SailingBoat> availableBoats = new List<SailingBoat>();

            foreach (var boat in boats)
            {
                bool available = true;
                foreach (var reserv in boat.SailingReservations)
                {
                    Console.WriteLine($"Checking {reserv.Date} - {reserv.Duration} on {reservationDate} - {duration} --> {DateChecker.AvailableOn(reserv.Date, reserv.Duration, reservationDate, duration)}");
                    if (!DateChecker.AvailableOn(reserv.Date, reserv.Duration, reservationDate, duration))
                    {
                        available = false;
                    }
                }

                if (available) availableBoats.Add(boat);

            }

            return availableBoats;
        }

        public List<SailingBoat> GetAvailableBoats(DateTime reservationDate, TimeSpan duration, int typeId)
        {
            var boats = _boatService.GetAllBoats(typeId);
            List<SailingBoat> availableBoats = new List<SailingBoat>();
            
            foreach(var boat in boats)
            {
                bool available = true;
                foreach(var reserv in boat.SailingReservations)
                {
                    Console.WriteLine($"Checking {reserv.Date} - {reserv.Duration} on {reservationDate} - {duration} --> {DateChecker.AvailableOn(reserv.Date, reserv.Duration, reservationDate, duration)}");
                    if (!DateChecker.AvailableOn(reserv.Date, reserv.Duration, reservationDate, duration))
                    {
                        available = false;
                    }
                }

                if (available) availableBoats.Add(boat);

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
