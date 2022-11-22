using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace QuoVadis.Authentication
{
    public class GrpcAuthenticationHandler : AuthenticationHandler<GrpcAuthenticationHandlerOptions>
    {
        public GrpcAuthenticationHandler(
            IOptionsMonitor<GrpcAuthenticationHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder, 
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            => Task.FromResult(AuthenticateResult.NoResult());

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            return base.HandleChallengeAsync(properties);
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            return base.HandleForbiddenAsync(properties);
        }
    }
}
