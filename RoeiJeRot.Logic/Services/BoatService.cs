using RoeiJeRot.Database.Database;
using System.Collections.Generic;
using System.Linq;

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
        /// <summary>
        /// Updates the boat stock status.
        /// </summary>
        /// <param name="boatId">The boat identifier.</param>
        /// <param name="status"></param>
        void UpdateBoatStatus(int boatId, BoatStatus status);

        List<SailingBoat> GetBoats();
    }

    public class BoatService : IBoatService
    {
        private readonly RoeiJeRotDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoatService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public BoatService(RoeiJeRotDbContext context)
        {
            _context = context;
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

        public List<SailingBoat> GetBoats() => _context.SailingBoats.ToList();

        
    }
}