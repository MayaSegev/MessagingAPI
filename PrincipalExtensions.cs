using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MessagingAPI
{
    public static class PrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            Claim idClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            int userId = 0;
            
            if (idClaim != null)
            {
                int.TryParse(idClaim.Value, out userId);
            }

            return userId;
        }
    }
}
