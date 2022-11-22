using Microsoft.AspNetCore.Authentication.Cookies;

namespace QuoVadis.Extensions
{
    public static class AuthenticationExtensions
    {
        public static CookieAuthenticationOptions DeactivateCookieChallenge(this CookieAuthenticationOptions options)
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;

                return Task.CompletedTask;
            };

            return options;
        }
    }
}
