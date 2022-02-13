using Tortuga.Chain;
using Tortuga.Chain.MySql;

namespace MessagingAPI.DAL
{
    public class DBRepo
    {
        private static readonly MySqlDataSource s_DataSource;
        private static readonly MySqlDataSource s_StrictDataSource;
        public static MySqlDataSource DataSource { get { return s_DataSource; } }
        static DBRepo()
        {
            string connString = ConfigurationHelper.config.GetSection("ConnectionStrings:MessagingDB").Value;
            s_DataSource = new MySqlDataSource(connString);
            s_StrictDataSource = s_DataSource.WithSettings(new MySqlDataSourceSettings() { StrictMode = true });
        }

        public static class Procedures
        {
            public const string
                ValidateUser = "sp_validateUser";

            public const string
                PostMessage = "sp_postMessage";

            public const string
                GetUser = "sp_getUser";

            public const string
                AddFollower = "sp_addFollower";

            public const string
                GetFollowFeed = "sp_getFollowFeed";

            public const string
                GetGlobalFeed = "sp_getGlobalFeed";

            public const string
                SetSignIn = "sp_setSignIn";

            public const string
               GetSignedInUsers = "sp_getSignedInUsers";
        }
    }
}
