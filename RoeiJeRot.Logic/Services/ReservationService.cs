using System;
using System.Collections.Generic;
using System.Linq;
using RoeiJeRot.Database.Database;

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