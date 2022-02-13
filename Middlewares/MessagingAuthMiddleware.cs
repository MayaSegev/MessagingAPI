using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace MessagingAPI.Middlewares
{
    public class MessagingAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public MessagingAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            int currentUserId = context.User.GetUserId();

            if (currentUserId == 0 || !SignedInUsers.GetSignedInUsers().IsSignedIn(currentUserId))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("User is not signed in");
                return;
            }

            // Call the next delegate/middleware
            await _next(context);
        }
    }
}
