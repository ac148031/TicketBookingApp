using System;
using System.Security.Cryptography;
using System.Text;

namespace TicketBookingApp
{
    public class PWSecurity
    {
        public static string Hash(string source)
        {
            string hash = BCrypt.Net.BCrypt.EnhancedHashPassword(source, 13);

            return hash.ToString();
        }

        public static bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }
    }
}
