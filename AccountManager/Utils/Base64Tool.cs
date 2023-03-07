using System;
using System.Text;

namespace AccountManager.Utils
{
    public static class Base64Tool
    {
        public static string Encode(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static string Decode(string text)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(text));
        }
    }
}