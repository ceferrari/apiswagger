using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProjetoPO.Api.Results;

namespace ProjetoPO.Api.Token
{
    public class TokenProviderMiddleware
    {
        private const string ContentType = "application/json";
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private readonly JsonSerializerSettings _serializerSettings;

        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options)
        {
            _next = next;

            _options = options.Value;
            ThrowIfInvalidOptions(_options);

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        public Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.Equals(_options.TokenPath, StringComparison.Ordinal))
            {
                return _next(context);
            }

            if (context.Request.Method.Equals("POST") && context.Request.HasFormContentType)
            {
                return GenerateToken(context);
            }

            context.Response.ContentType = ContentType;
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new BadRequestOperationResult(), _serializerSettings));
        }

        private async Task GenerateToken(HttpContext context)
        {
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];

            var identity = await _options.IdentityResolver(username, password);
            if (identity == null)
            {
                context.Response.ContentType = ContentType;
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new BadRequestOperationResult("Usuário e/ou senha inválido(s)."), _serializerSettings));
                return;
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, await _options.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var jwt = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                _options.NotBefore,
                _options.Expiration,
                _options.SigningCredentials);

            var response = new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwt),
                ExpiresIn = (int)_options.ValidFor.TotalSeconds
            };

            context.Response.ContentType = ContentType;
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, _serializerSettings));
            //await context.Response.WriteAsync(JsonConvert.SerializeObject(new OkOperationResult("Token de acesso obtida com sucesso!", response), _serializerSettings));
        }

        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrEmpty(options.TokenPath))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.TokenPath));
            }

            if (string.IsNullOrEmpty(options.Issuer))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));
            }

            if (string.IsNullOrEmpty(options.Audience))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));
            }

            if (options.Expiration == null || options.Expiration == DateTime.MinValue)
            {
                throw new ArgumentException(nameof(TokenProviderOptions.Expiration));
            }
            
            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.JtiGenerator));
            }

            if (options.IdentityResolver == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.IdentityResolver));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));
            }
        }
    }
}