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
        /// <summary>
        /// Returns a list of boats which can be reserved on the given date. 
        /// </summary>
        /// <param name="reservationDate"></param>
        /// <param name="duration"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        List<SailingBoat> GetAvailableBoats(DateTime reservationDate, TimeSpan duration, int typeId);
        /// <summary>
        /// Places an new boat reservation on the given date with duration.
        /// </summary>
        bool PlaceReservation(int boatType, int memberId, DateTime reservationDate, TimeSpan duration);
        /// <summary>
        /// Returns all reservations from the current data 
        /// </summary>
        /// <returns>All sailingReservations</returns>
        List<SailingReservation> GetReservations();

        /// <summary>
        /// Cancels a boat reservation.
        /// </summary>
        /// <param name="reservationId"></param>
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


        /// <inheritdoc />
        public bool PlaceReservation(int boatType, int memberId, DateTime reservationDate, TimeSpan duration)
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
                    ReservedByUserId = memberId,
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
            var availableBoats = new List<SailingBoat>();
            
            foreach(var boat in boats)
            {
                bool available = true;
                foreach(var reservation in boat.SailingReservations)
                {
                    Console.WriteLine($"Checking {reservation.Date} - {reservation.Duration} on {reservationDate} - {duration} --> {DateChecker.AvailableOn(reservation.Date, reservation.Duration, reservationDate, duration)}");
                    if (!DateChecker.AvailableOn(reservation.Date, reservation.Duration, reservationDate, duration))
                    {
                        available = false;
                    }
                }

                if (available) availableBoats.Add(boat);

            }

            return availableBoats;
        }
        
        /// <inheritdoc />
        public List<SailingReservation> GetReservations()
        {
            return _context.Reservations.Where(x => x.Date >= DateTime.Now.Date).ToList();
        }

        /// <inheritdoc />
        public void CancelBoatReservation(int reservationId)
        {
            throw new NotImplementedException();
        }
    }
}
