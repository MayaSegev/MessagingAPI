using MessagingAPI.DAL;
using MessagingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingAPI
{
    public class SignedInUsers
    {
        public Dictionary<int, DateTime> Users { get; set; }

        private SignedInUsers()
        {
            this.Users = new Dictionary<int, DateTime>();
        }

        private static readonly SignedInUsers data = new SignedInUsers();

        public static SignedInUsers GetSignedInUsers()
        {
            return data;
        }

        public async Task LoadData()
        {
            // Allowed only when empty
            if (Users.Count == 0)
            {
                DBManager dbManger = new DBManager();
                this.Users = await dbManger.GetSignedInUsers();
            }
        }

        public bool IsSignedIn(int userId)
        {
            return this.Users.ContainsKey(userId);
        }
    }
}
