using System.Security.Cryptography;
using System.Text;

namespace AccountManager.Utils
{
    public static class ShaTool
    {
        public static byte[] GetSha256Hash(string data)
        {
            SHA256 sha = new SHA256Managed();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(data));
        }
    }
}
