using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set;  }
        public DateTime LastSignIn { get; set; }
    }
}
