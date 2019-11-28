using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RoeiJeRot.Database.Database;

namespace RoeiJeRot.Logic.Services
{
    /// <summary>
    ///     Interface for logic that retrieves and updates boats from database.
    /// </summary>
    public interface IBoatService
    {
        /// <summary>
        ///     Returns a list of all boats.
        /// </summary>
        /// <returns>All boats</returns>
        List<SailingBoat> GetAllBoats();

        /// <summary>
        ///     Returns a list of all boats with the given typeId.
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns>Returns all boats of given typeId</returns>
        List<SailingBoat> GetAllBoats(int typeId);

        /// <summary>
        ///     Updates the boat stock status.
        /// </summary>
        /// <param name="boatId">The boat identifier.</param>
        /// <param name="status"></param>
        void UpdateBoatStatus(int boatId, BoatState status);

        /// <summary>
        ///     Returns a list of boats which can be reserved on the given date.
        /// </summary>
        /// <param name="reservationDate"></param>
        /// <param name="duration"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        List<SailingBoat> GetAvailableBoats(DateTime reservationDate, TimeSpan duration);
    }

    public class BoatService : IBoatService
    {
        private readonly RoeiJeRotDbContext _context;

        public BoatService(RoeiJeRotDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public List<SailingBoat> GetAllBoats()
        {
            return _context.SailingBoats.Include(x => x.SailingReservations).Include(x => x.BoatType).ToList();
        }

        /// <inheritdoc />
        public List<SailingBoat> GetAllBoats(int typeId)
        {
            return GetAllBoats()
                .Where(boat => boat.BoatTypeId == typeId)
                .ToList();
        }

        /// <inheritdoc />
        public void UpdateBoatStatus(int boatId, BoatState status)
        {
            var boat = _context.SailingBoats.FirstOrDefault(b => b.Id == boatId);

            if (boat != null) boat.Status = (int) status;

            _context.SaveChanges();
        }

        public List<SailingBoat> GetAvailableBoats(DateTime reservationDate, TimeSpan duration)
        {
            var boats = GetAllBoats();
            var availableBoats = new List<SailingBoat>();

            foreach (var boat in boats)
            {
                var available = true;
                foreach (var reserv in boat.SailingReservations)
                {
                    Console.WriteLine(
                        $"Checking {reserv.Date} - {reserv.Duration} on {reservationDate} - {duration} --> {DateChecker.AvailableOn(reserv.Date, reserv.Duration, reservationDate, duration)}");
                    if (!DateChecker.AvailableOn(reserv.Date, reserv.Duration, reservationDate, duration))
                        available = false;
                }

                if (available) availableBoats.Add(boat);
            }

            return availableBoats;
        }

        /// <inheritdoc />
        public List<SailingBoat> GetBoats()
        {
            return _context.SailingBoats.Include(x => x.BoatType).ToList();
        }
    }
}