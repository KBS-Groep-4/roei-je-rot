using System.Text;

namespace RoeiJeRot.Logic
{
    class PasswordHasher
    {
        public bool CompareHash(string dbHash, string password)
        {
            bool value = false;

            if (dbHash == GetPasswordHash(password))
            {
                value = true;
            }

            return value;
        }

        public string GetPasswordHash(string password)
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
