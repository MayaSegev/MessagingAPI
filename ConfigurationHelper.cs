using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingAPI
{
    public static class ConfigurationHelper
    {
        public static IConfiguration config;

        public static string SigningKey;

        public static int TokenDurationInHours;

        public static int MessageLimit;

        public static string PasswordKeyEncryption;

        public static void Initialize(IConfiguration Configuration)
        {
            config = Configuration;
            SigningKey = config.GetSection("Signing:Key").Value;
            
            int durationInHours = 24;
            int.TryParse(config.GetSection("Signing:DurationInHours").Value, out durationInHours);
            TokenDurationInHours = durationInHours;

            int messageLimit = 140;
            int.TryParse(config.GetSection("Messaging:Limit").Value, out messageLimit);
            MessageLimit = messageLimit;

            PasswordKeyEncryption = config.GetSection("PasswordEncryption:Key").Value;
        }
    }
}
