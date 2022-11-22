using Grpc.Core;

namespace QuoVadis.Extensions
{
    public static class ServerCallContextExtensions
    {
        public static string? GetIdentityUsername(this ServerCallContext context) => context.GetHttpContext().User.Identity?.Name;
    }
}
