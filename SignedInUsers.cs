using MessagingAPI.DAL;
using MessagingAPI.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingAPI
{
    public class SignedInUsers
    {
        public ConcurrentDictionary<int, DateTime> Users { get; set; }
        private static object lockObj = new object();
        private static SignedInUsers data;

        private SignedInUsers()
        {
            this.Users = new ConcurrentDictionary<int, DateTime>();
        }

        public static SignedInUsers GetSignedInUsers
        {
            get
            {
                if (data == null)
                {
                    lock (lockObj)
                    {
                        if (data == null)
                            data = new SignedInUsers();
                    }
                }

                return data;
            }
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
