using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using ProjetoPO.Api.Contexts;
using ProjetoPO.Api.Filters;
using ProjetoPO.Api.Token;
using Swashbuckle.AspNetCore.Swagger;

namespace ProjetoPO.Api
{
    public partial class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("TokenProviderOptions:SecretKey").Value));

            _tokenProviderOptions = new TokenProviderOptions
            {
                TokenPath = Configuration.GetSection("TokenProviderOptions:TokenPath").Value,
                Audience = Configuration.GetSection("TokenProviderOptions:Audience").Value,
                Issuer = Configuration.GetSection("TokenProviderOptions:Issuer").Value,
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            };

            _tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = _signingKey,
                ValidAudience = Configuration.GetSection("TokenProviderOptions:Audience").Value,
                ValidIssuer = Configuration.GetSection("TokenProviderOptions:Issuer").Value,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BaseContext>(opt => opt.UseInMemoryDatabase("ProjetoPO"));

            services.AddOptions();

            ConfigureAuth(services);

            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddMvc(config =>
            {
                config.Filters.Add(new CustomAuthorizeFilter(
                    new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build()));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "ProjetoPO",
                    Description = "Exemplo de API's RESTful como trabalho final da matéria Sistemas Distribuídos",
                    TermsOfService = "https://www.projetopo.com.br/termos",
                    Contact = new Contact
                    {
                        Name = "Carlos Eduardo Ferrari e Vitor Carnello Jatobá",
                        Email = "projetopo@projetopo.com.br",
                        Url = "https://www.projetopo.com.br"
                    },
                    License = new License
                    {
                        Name = "Licensed under the Apache License, Version 2.0",
                        Url = "http://www.apache.org/licenses/LICENSE-2.0"
                    }
                });

                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "ProjetoPO.Api.xml"));

                c.MapType<Guid>(() => new Schema
                {
                    Type = "string",
                    Format = "uuid"
                });

                //c.OrderActionsBy(_sortKeySelector);

                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "password",
                    TokenUrl = "http://localhost:12345/api/token"
                });

                //c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                //{
                //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {access_token}\"",
                //    Name = "Authorization",
                //    In = "header"
                //});
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            _tokenProviderOptions.IdentityResolver = (username, password) => GetIdentity(app, username, password);
            app.UseTokenProvider(_tokenProviderOptions);
            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjetoPO v1");
                c.InjectStylesheet("/swagger-ui/custom.css");
            });
        }

        private Func<ApiDescription, string> _sortKeySelector = x => 
            x.HttpMethod.Equals("GET", StringComparison.InvariantCultureIgnoreCase)
                ? "0"
                : x.HttpMethod.Equals("POST", StringComparison.InvariantCultureIgnoreCase)
                    ? "1"
                    : x.HttpMethod.Equals("PUT", StringComparison.InvariantCultureIgnoreCase)
                        ? "2"
                        : x.HttpMethod.Equals("DELETE", StringComparison.InvariantCultureIgnoreCase)
                            ? "3"
                            : "4";
    }
}
