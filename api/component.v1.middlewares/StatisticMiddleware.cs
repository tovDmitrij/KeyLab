using Microsoft.AspNetCore.Http;

namespace component.v1.middlewares
{
    public sealed class StatisticMiddleware(RequestDelegate request)
    {
        private readonly RequestDelegate _request = request;

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Cookies.ContainsKey("stats_uuid"))
            {
                Guid guid = Guid.NewGuid();
                context.Response.Cookies.Append("stats_uuid", guid.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddYears(25)
                });
            }

            await _request(context);
        }
    }
}