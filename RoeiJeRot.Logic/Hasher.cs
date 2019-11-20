using System.Text;

namespace RoeiJeRot.Logic
{
    static class Hasher
    {
        /// <summary>
        /// Compares the specified database hash.
        /// </summary>
        /// <param name="dbHash">The database hash.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// Returns true if hashes are equal, else returns false.
        /// </returns>
        public static bool Compare(string dbHash, string password)
        {
            return dbHash == Hash(password);
        }

        /// <summary>
        /// Hashes the specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        /// Returns password hash as a string.
        /// </returns>
        public static string Hash(string password)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new StringBuilder();

            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password));
            foreach (byte b in crypto)
            {
                hash.Append(b.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
