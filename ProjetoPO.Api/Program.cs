using System;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjetoPO.Api.Contexts;
using ProjetoPO.Api.Models;

namespace ProjetoPO.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetService<BaseContext>();
                    if (context.Users.Any())
                    {
                        return;
                    }

                    context.Users.AddRange
                    (
                        new User
                        {
                            Id = Guid.NewGuid(),
                            Username = "sabatine",
                            Password = "prof456"
                        },
                        new User
                        {
                            Id = Guid.NewGuid(),
                            Username = "carlos",
                            Password = "teste123"
                        },
                        new User
                        {
                            Id = Guid.NewGuid(),
                            Username = "vitor",
                            Password = "321master"
                        }
                    );

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
