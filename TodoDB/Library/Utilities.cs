using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TodoDB.Library
{
    public class Utilities
    {
        //protected internal string GetSHA512(string str, string secretKey)
        //{

        //    string computedHash = "";

        //    using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(secretKey)))
        //    {
        //        computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str)).ToString();
        //    }

        //    return computedHash;
        //}


        protected internal string GetSHA512(string value, string key)
        {
            //if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("value");
            //if (String.IsNullOrWhiteSpace(key)) throw new ArgumentNullException("key");

            var valueBytes = System.Text.Encoding.UTF8.GetBytes(value);
            var keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

            var alg = new System.Security.Cryptography.HMACSHA512(keyBytes);
            var hash = alg.ComputeHash(valueBytes);

            var result = //Crypto.BinaryToHex(hash);
                        Convert.ToBase64String(hash, 0, hash.Length);
            return result;
        }

        //protected internal string GetSHA512(string randomString)
        //{
        //    var crypt = new SHA512Managed();
        //    string hash = String.Empty;
        //    byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
        //    foreach (byte theByte in crypto)
        //    {
        //        hash += theByte.ToString("x2");
        //    }

        //    return hash;
        //}

    }
}
