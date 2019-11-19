using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using RoeiJeRot.Database.Database;

namespace RoeiJeRot.Logic.Services
{
    public interface IUserService
    {
        List<User> GetUsersExample();
    }

    public class UserService : IUserService
    {
        private readonly RoeiJeRotDbContext _context;

        public UserService(RoeiJeRotDbContext context)
        {
            _context = context;
        }

        /// <summary>
        ///  Voorbeeld voor get users database query.
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsersExample()
        {
            return _context.Users
                .Where(x => x.FirstName == "Pauul")
                .ToList();
        }
    }
}
