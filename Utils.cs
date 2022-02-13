using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingAPI
{
    public static class Utils
    {
        public static string Encrypt(string value) 
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            value += ConfigurationHelper.PasswordKeyEncryption;
            var passwordBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(passwordBytes);
        }

        public static string Decrypt(string base64EncodeData)
        {
            if (string.IsNullOrEmpty(base64EncodeData)) return string.Empty;
            var base64EncodeBytes = Convert.FromBase64String(base64EncodeData);
            var result = Encoding.UTF8.GetString(base64EncodeBytes);
            result = result.Substring(0, result.Length - ConfigurationHelper.PasswordKeyEncryption.Length);
            return result;
        }
    }
}
