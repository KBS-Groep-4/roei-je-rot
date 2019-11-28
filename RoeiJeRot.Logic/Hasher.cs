using System.Security.Cryptography;
using System.Text;

namespace RoeiJeRot.Logic
{
    /// <summary>
    ///     Class to compare hashes from the database to the password the user inputs.
    /// </summary>
    public static class Hasher
    {
        /// <summary>
        ///     Compares the specified database hash.
        /// </summary>
        /// <param name="dbHash">The database hash.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///     Returns true if hashes are equal, else returns false.
        /// </returns>
        public static bool Compare(string dbHash, string password)
        {
            return dbHash == Hash(password);
        }

        /// <summary>
        ///     Hashes the specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        ///     Returns password hash as a string.
        /// </returns>
        public static string Hash(string password)
        {
            var crypt = new SHA256Managed();
            var hash = new StringBuilder();

            var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password));
            foreach (var b in crypto) hash.Append(b.ToString("x2"));
            return hash.ToString();
        }
    }
}