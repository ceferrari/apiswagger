using Microsoft.EntityFrameworkCore;
using ProjetoPO.Api.Models;

namespace ProjetoPO.Api.Contexts
{
    public class BaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public BaseContext(DbContextOptions<BaseContext> options)
            : base(options)
        {

        }
    }
}
