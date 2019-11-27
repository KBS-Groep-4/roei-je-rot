using System;
using System.Collections.Generic;
﻿using RoeiJeRot.Database.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RoeiJeRot.Logic.Services
{
    /// <summary>
    /// Interface for logic that retrieves and updates boats from database.
    /// </summary>
    public enum BoatStatus
    {
        InStock = 0,
        InUse = 1,
        InService = 2
    }

    public interface IBoatService
    {
        List<SailingBoat> GetAllBoats();
        List<SailingBoat> GetAllBoats(int typeId);

        /// <summary>
        /// Updates the boat stock status.
        /// </summary>
        /// <param name="boatId">The boat identifier.</param>
        /// <param name="status"></param>
        void UpdateBoatStatus(int boatId, BoatStatus status);
    }
    public class BoatService : IBoatService
    {
        private readonly RoeiJeRotDbContext _context;
        public BoatService(RoeiJeRotDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gives all the boats no matter of status
        /// </summary>
        /// <returns>All boats</returns>
        public List<SailingBoat> GetAllBoats()
        {
            return _context.SailingBoats.Include(x => x.SailingReservations).ToList();
        }

        /// <summary>
        /// Get all sailboats with by given typeId regardless of status
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns>Returns all boats of given typeId</returns>
        public List<SailingBoat> GetAllBoats(int typeId)
        {
            return GetAllBoats()
                .Where(boat => boat.BoatTypeId == typeId)
                .ToList();
        }

        public void UpdateBoatStatus(int boatId, BoatStatus status)
        {
            var boat = _context.SailingBoats.FirstOrDefault(b => b.Id == boatId);

            if (boat != null)
            {
                boat.Status = (int)status;
            }

            _context.SaveChanges();
        }
    }
}