using System.Collections.Generic;
using System.Linq;
using RoeiJeRot.Database.Database;

namespace RoeiJeRot.Logic.Services
{
    /// <summary>
    ///     Interface for logic that retrieves user data.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        ///     Returns a list of users.
        /// </summary>
        /// <returns></returns>
        List<User> GetUsers();
    }

    public class UserService : IUserService
    {
        private readonly RoeiJeRotDbContext _context;

        public UserService(RoeiJeRotDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }
    }
}