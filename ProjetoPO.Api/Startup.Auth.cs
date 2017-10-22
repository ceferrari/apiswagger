using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProjetoPO.Api.Contexts;
using ProjetoPO.Api.Token;

namespace ProjetoPO.Api
{
    public partial class Startup
    {
        private readonly SymmetricSecurityKey _signingKey;
        private readonly TokenProviderOptions _tokenProviderOptions;
        private readonly TokenValidationParameters _tokenValidationParameters;

        private void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = _tokenValidationParameters;
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = Configuration.GetSection("TokenProviderOptions:CookieName").Value;
                    options.TicketDataFormat = new CustomJwtDataFormat(SecurityAlgorithms.HmacSha256, _tokenValidationParameters);
                });
        }

        private Task<ClaimsIdentity> GetIdentity(IApplicationBuilder app, string username, string password)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<BaseContext>();

            return context.Users.FirstOrDefault(x => x.Username == username && x.Password == password) == null
                ? Task.FromResult<ClaimsIdentity>(null)
                : Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));
        }
    }
}
