using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Util
{
    public static class SHATool
    {
        public static byte[] GetSHA256Hash(string data)
        {
            SHA256 sha = new SHA256Managed();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        public static byte[] GetSHA1Hash(string data)
        {
            SHA1 sha = new SHA1Managed();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(data));
        }
    }
}
