﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace TicketBookingApp
{
    public class PWSecurity
    {
        public static string Hash(string source)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(source, 13);
        }

        public static bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }
    }
}
