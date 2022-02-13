using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingAPI.Models
{
    public class Follower
    {
        public int FollowerUserId { get; set; }
        public int FollowingUserId { get; set; }
        public DateTime FollowingSince { get; set; }
    }
}
