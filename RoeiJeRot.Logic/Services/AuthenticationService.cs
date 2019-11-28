using System;
using System.Linq;

namespace RoeiJeRot.Logic.Services
{
    public interface IAuthenticationService
    {
        /// <summary>
        ///     Returns if the given username and password is a valid user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>
        ///     Returns true if record with same username and password is found, else returns false.
        /// </returns>
        bool AuthenticateUser(string username, string password);

        /// <summary>
        ///     Creates an account with the given username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        void CreateAccount(string username, string password);

        /// <summary>
        ///     Removes an account by the given username.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        void RemoveAccount(string username);

        /// <summary>
        ///     Updates an account with the given information.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="streedName"></param>
        /// <param name="houseNmr"></param>
        /// <param name="postalCode"></param>
        void UpdateAccount(string username, string password, string firstname, string lastname, string streedName,
            string houseNmr, string postalCode);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;

        public AuthenticationService(IUserService userService)
        {
            _userService = userService;
        }

        /// <inheritdoc />
        public bool AuthenticateUser(string username, string password)
        {
            return _userService
                .GetUsers()
                .Any(u => u.Username == username && Hasher.Compare(u.Password, password));
        }

        /// <inheritdoc />
        public void CreateAccount(string username, string password)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void RemoveAccount(string username)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void UpdateAccount(string username, string password, string firstname, string lastname,
            string streedName, string houseNmr, string postalCode)
        {
            throw new NotImplementedException();
        }
    }
}