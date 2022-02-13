using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MessagingAPI.DAL;
using MessagingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MessagingAPI.Controllers
{
    public class TokenController : Controller
    {
        public class LoginData
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        [Route("/SignIn")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoginData data)
        {
            DBManager dbManger = new DBManager();
            User authUser = await dbManger.ValidateUser(data.username, Utils.Encrypt(data.password));
            if (authUser != null)
            {
                if (!SignedInUsers.GetSignedInUsers().Users.ContainsKey(authUser.Id))
                {
                    SignedInUsers.GetSignedInUsers().Users.Add(authUser.Id, DateTime.Now);
                }
                else
                {
                    SignedInUsers.GetSignedInUsers().Users[authUser.Id] = DateTime.Now;
                }

                return new ObjectResult(GenerateToken(authUser));
            }
            else
            {
                return BadRequest();
            }
        }


        private dynamic GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddHours(ConfigurationHelper.TokenDurationInHours)).ToUnixTimeSeconds().ToString())
            };

            var token = new JwtSecurityToken(
                   new JwtHeader(
                        new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationHelper.SigningKey)), 
                            SecurityAlgorithms.HmacSha256)), 
                    new JwtPayload(claims));

           return new
            {
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = user.Username
           };
        }
    }
}
