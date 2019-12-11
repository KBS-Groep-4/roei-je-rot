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
        void CancelReservation(int reservationId);

        /// <summary>
        /// Re-allocates the future reservations of a boat
        /// </summary>
        /// <param name="boatId">Id of the boat to be cancelled</param>
        /// <returns>A list of reservations that could not be reallocated</returns>
        List<SailingReservation> AllocateBoatReservations(int boatId);

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
            var availableBoats = _boatService.GetAvailableBoats(reservationDate, duration).Where(boat => boat.BoatTypeId == boatType && boat.Status != (int)BoatState.InService).ToList();

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
                    {
                        boatToReserve = boat;
                        max = boat.SailingReservations.Count;
                    }
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
            return user.Reservations.Where(reserv => (reserv.Date + reserv.Duration) >= DateTime.Now && reserv.ReservedByUserId == memberId).ToList();
        }

        /// <inheritdoc />
        public List<SailingReservation> GetReservations()
        {
            return _context.Reservations.Where(x => x.Date >= DateTime.Now.Date).ToList();
        }

        /// <inheritdoc />
        public void CancelReservation(int reservationId)
        {
            var reservations = _context.Reservations.Where(reserv => reserv.Id == reservationId);
            SailingReservation reservation;
            if (reservations.Any())
            {
                reservation = reservations.First();
                _context.Remove(reservation);
                _context.SaveChanges();
            }
            else throw new Exception("No reservation of this id found");
        }

        /// <inheritdoc />
        public List<SailingReservation> AllocateBoatReservations(int boatId)
        {
            // Get all future reservations that need to be cancelled
            var reservationsToCancel =
                _context.Reservations.Where(reserv => reserv.ReservedSailingBoatId == boatId && reserv.Date > DateTime.Now).ToList();

            // List of boats that could not be allocated
            List<SailingReservation> notReAllocatable = new List<SailingReservation>();

            // Place for each reservation a new reservation, if one could not be placed they are put in not Can
            foreach (SailingReservation reservation in reservationsToCancel)
            {
                int boatType = reservation.ReservedSailingBoat.BoatTypeId;
                int boatUser = reservation.ReservedByUserId;
                DateTime reservationDate = reservation.Date;
                TimeSpan reservationDuration = reservation.Duration;

                if(!PlaceReservation(boatType, boatUser, reservationDate, reservationDuration)) 
                    notReAllocatable.Add(reservation);

                _context.Remove(reservation);
                _context.SaveChanges();
            }

            return notReAllocatable;
        }
    }
}
