using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace OrgCommunication.Helpers.Security
{
    public static class SecurityHelper
    {
        private static RNGCryptoServiceProvider _cryptoServiceProvider = new RNGCryptoServiceProvider();
        private const int SALT_SIZE = 24;

        private static RNGCryptoServiceProvider RNGCrypto
        {
            get { return _cryptoServiceProvider; }
        }

        public static string GenerateBase64SaltString()
        {
            // Lets create a byte array to store the salt bytes
            byte[] saltBytes = new byte[SecurityHelper.SALT_SIZE];

            // lets generate the salt in the byte array
            SecurityHelper.RNGCrypto.GetNonZeroBytes(saltBytes);

            // Let us get some string representation for this salt
            string saltString = Convert.ToBase64String(saltBytes);

            return saltString;
        }

        public static string ComputeBase64Hash(string plaintext)
        {
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] dataBytes = System.Text.UTF8Encoding.UTF8.GetBytes(plaintext);
            byte[] resultBytes = sha.ComputeHash(dataBytes);

            return Convert.ToBase64String(resultBytes);
        }
    }
}