using System.Security.Cryptography;
using System.Text;

namespace QuoVadis.Common.Extensions
{
    public static class StringExtensions
    {
        public static string HashWithSha512(this string value)
        {
            using var sha = SHA512.Create();

            var valueBuffer = Encoding.UTF8.GetBytes(value);
            var hashRaw = sha.ComputeHash(valueBuffer);

            return Convert.ToBase64String(hashRaw);
        }
    }
}
