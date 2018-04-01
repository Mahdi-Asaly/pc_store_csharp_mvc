using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace PcStoreWebsite
{
    public class Encryption
    {
        public const int SALT_SIZE = 24;
        public const int HASH_SIZE = 24;
        public const int PBKDF2_ITT = 500;
        public string CreateHash(string password)
        {
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_SIZE];
            csprng.GetBytes(salt);

            //generate the password hash
            byte[] hash = PBKDF2(password, salt, PBKDF2_ITT, HASH_SIZE);
            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }
        private byte[] PBKDF2(string password, byte[] salt , int pBKDF2_ITT,int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = pBKDF2_ITT;
            return pbkdf2.GetBytes(outputBytes);
        }
        private bool SlowEquals(byte[] dbHash, byte[] passHash)
        {
            uint diff = (uint)dbHash.Length ^ (uint)passHash.Length;
            for (int i = 0; i < dbHash.Length && i < passHash.Length; i++)
                diff |= (uint)dbHash[i] ^ (uint)passHash[i];
            return diff == 0;
        }
        public bool ValidatePassword(string password , string dbHash)
        {
            char[] delimiter = { ':' };
            string[] split = dbHash.Split(delimiter);
            byte[] salt = Convert.FromBase64String(split[0]);
            byte[] hash = Convert.FromBase64String(split[1]);

            byte[] hashToValidate = PBKDF2(password, salt, PBKDF2_ITT, hash.Length);
            return SlowEquals(hash, hashToValidate);
        }

    }

    
}