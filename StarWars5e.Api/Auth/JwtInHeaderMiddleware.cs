using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StarWars5e.Api.Auth
{
    public class JwtInHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtInHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            const string name = "sw5e_token";
            var cookie = context.Request.Cookies[name];

            if (cookie != null)
            {
                if (!context.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Request.Headers.Append("Authorization", "Bearer " + cookie);
                }
            }

            await _next.Invoke(context);
        }
    }
}
