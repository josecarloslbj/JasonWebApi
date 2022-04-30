using Microsoft.EntityFrameworkCore;

namespace Jason.WebApi
{
    public class AppDbContext : DbContext
    {        
        public DbSet<VersionInfo> VersionInfo { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
