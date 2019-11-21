using System;
using System.Collections.Generic;
using System.Text;
using RoeiJeRot.Database.Database;
using System.Linq;

namespace RoeiJeRot.Logic.Services
{
    public interface IBoatService
    {
        List<SailingBoat> GetAllBoats();
    }
    class BoatService : IBoatService
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
            return _context.SailingBoats.ToList();
        }

        /// <summary>
        /// Get all sailboats with by given typeId regardless of status
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns>Returns all boats of given typeId</returns>
        public List<SailingBoat> GetAllBoats(int typeId)
        {
            return GetAllBoats().Where(boat => boat.typeId).ToList();
        }
    }
}
