using System.Collections.Generic;
using System.Threading.Tasks;
using MessagingAPI.DAL;
using MessagingAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagingController : ControllerBase
    {
        private int currentUserId;
        DBManager dbManger;

        public MessagingController(IHttpContextAccessor httpContextAccessor)
        {
            currentUserId = httpContextAccessor.HttpContext.User.GetUserId();
            dbManger = new DBManager();
        }

        [HttpGet]
        [Route("")]
        public string Get()
        {
            return "Welcome to social messaging api";
        }

        [HttpPost]
        [Route("PostMessage")]
        public async Task<ObjectResult> PostMessage([FromBody] string message)
        {
            if (message.Length > ConfigurationHelper.MessageLimit)
                return UnprocessableEntity($"Message exceeded {ConfigurationHelper.MessageLimit} characters limit");
            
            
            Message res = await dbManger.PostMessage(this.currentUserId, message);
            if (res == null)
            {
                return StatusCode(500, "Failed to post message");
            }

            return Ok(res); 
        }

        [HttpPut]
        [Route("Follow/{userToFollow}")]
        public async Task<ObjectResult> Follow(string userToFollow)
        {
            var res = await dbManger.GetUser(userToFollow);
            if (res == null) 
            {
                return NotFound($"Cannot follow {userToFollow}, user does not exist");
            }

            Follower addedFollower = await dbManger.AddFollower(this.currentUserId, res.Id);
            if (addedFollower == null)
            {
                return StatusCode(500, $"Failed to follow {res.Username}");
            }

            return Ok(addedFollower);
        }

        [HttpGet]
        [Route("GetFollowFeed")]
        public async Task<ObjectResult> FollowFeed()
        {
            List<Message> res = await dbManger.GetFollowFeed(this.currentUserId);

            if (res == null)
            {
                return StatusCode(500, $"Error getting follow feed");
            }

            return Ok(res); 
        }

        [HttpGet]
        [Route("GetGlobalFeed")]
        public async Task<ObjectResult> Feed()
        {
            List<Message> res = await dbManger.GetGlobalFeed();

            if (res == null)
            {
                return StatusCode(500, $"Error getting global feed");
            }

            return Ok(res);
        }

        [HttpPut]
        [Route("SignOut")]
        public async Task<ObjectResult> SignOut()
        { 
            bool res = await dbManger.SignOut(this.currentUserId);

            if (res)
            {
                if (SignedInUsers.GetSignedInUsers().Users.ContainsKey(this.currentUserId))
                {
                    SignedInUsers.GetSignedInUsers().Users.Remove(this.currentUserId);
                }

                return Ok(res);
            }

            return NotFound(res);
        }
    }
}
