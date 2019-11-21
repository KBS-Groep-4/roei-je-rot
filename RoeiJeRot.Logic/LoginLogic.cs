using RoeiJeRot.Logic.Services;
using System.Linq;

namespace RoeiJeRot.Logic
{
    /// <summary>
    /// Logic for login related tasks.
    /// </summary>
    public class LoginLogic
    {
        private readonly IUserService _userService;

        public LoginLogic(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Compares given input to database records.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>
        /// Returns true if record with same username and password is found, else returns false.
        /// </returns>
        public bool AuthenticateUser(string username, string password)
        {
            return _userService
                        .GetUsers()
                        .Any(u => u.Username == username && Hasher.Compare(u.Password, password));
        }
    }
}