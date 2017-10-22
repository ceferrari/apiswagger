using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace ProjetoPO.Api.Token
{
    public static class TokenProviderMiddlewareExtension
    {
        public static IApplicationBuilder UseTokenProvider(this IApplicationBuilder builder, TokenProviderOptions parameters)
        {
            return builder.UseMiddleware<TokenProviderMiddleware>(Options.Create(parameters));
        }
    }
}
