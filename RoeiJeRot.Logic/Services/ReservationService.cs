using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using RoeiJeRot.Database.Database;
using RoeiJeRot.Logic.Helper;

namespace RoeiJeRot.Logic.Services
{
    public interface IReservationService
    {
        /// <summary>
        ///     Places an new boat reservation on the given date with duration.
        /// </summary>
        bool PlaceReservation(int boatType, int memberId, DateTime reservationDate, TimeSpan duration);

        /// <summary>
        ///     Returns all reservations from the current data
        /// </summary>
        /// <returns>All sailingReservations</returns>
        List<SailingReservation> GetReservations();

        /// <summary>
        ///     Cancels a boat reservation.
        /// </summary>
        /// <param name="reservationId"></param>
        void CancelBoatReservation(int reservationId);

        List<SailingReservation> GetFutureReservations(int memberId);
    }

    public class ReservationService : IReservationService
    {
        private readonly IBoatService _boatService;
        private readonly RoeiJeRotDbContext _context;

        public ReservationService(RoeiJeRotDbContext context, IBoatService boatService)
        {
            _context = context;
            _boatService = boatService;
        }


        /// <inheritdoc />
        public bool PlaceReservation(int boatType, int memberId, DateTime reservationDate, TimeSpan duration)
        {
            var availableBoats = _boatService.GetAvailableBoats(reservationDate, duration);

            // Checks if the reservation doesn't violate any constraints
            ReservationConstraintsMessage message = ReservationConstraints.IsValid(reservationDate, duration, this, memberId);
            if (!message.IsValid) return false;
            
            // Check if there is an available boat
            if (availableBoats.Count > 0)
            {
                SailingBoat boatToReserve = null;

                // Take the boat with most reservations
                var max = int.MinValue;
                foreach (var boat in availableBoats)
                {
                    if (boat.SailingReservations.Count >= max)
                        boatToReserve = boat;
                }

                //Create a reservation for this boat
                _context.Reservations.Add(new SailingReservation
                {
                    Date = reservationDate,
                    Duration = duration,
                    ReservedByUserId = memberId,
                    ReservedSailingBoatId = boatToReserve.Id
                });

                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public List<SailingReservation> GetFutureReservations(int memberId)
        {
            var user = _context.Users.Where(user => user.Id == memberId).ToList()[0];
            return user.Reservations.Where(reserv => (reserv.Date + reserv.Duration) >= DateTime.Now).ToList();
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