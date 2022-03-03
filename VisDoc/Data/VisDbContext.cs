using Microsoft.EntityFrameworkCore;
using VisDoc.Models;
namespace VisDoc.Data
{
    public class VisDbContext : DbContext
    {
      
        public VisDbContext(DbContextOptions<VisDbContext> options) : base(options)
        {
        }
        public DbSet<DocumentModel> Document { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("AppDb");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
