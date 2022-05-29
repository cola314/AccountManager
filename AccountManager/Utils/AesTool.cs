using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AccountManager.Util
{
    internal static class AesTool
    {
        private static Rfc2898DeriveBytes CreateKeyBytes(string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] saltBytes = ShaTool.GetSha256Hash(key);
            return new Rfc2898DeriveBytes(keyBytes, saltBytes, 541);
        }

        public static string Decrypt(string input, string key)
        {
            var keyBytes = CreateKeyBytes(key);
            var aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = keyBytes.GetBytes(32);
            aes.IV = keyBytes.GetBytes(16);

            var decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            return Encoding.UTF8.GetString(xBuff);
        }

        public static string Encrypt(string input, string key)
        {
            var keyBytes = CreateKeyBytes(key);
            var aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = keyBytes.GetBytes(32);
            aes.IV = keyBytes.GetBytes(16);

            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            return Convert.ToBase64String(xBuff);
        }
    }
}
