using Microsoft.EntityFrameworkCore;
using VisDoc.Models;
namespace VisDoc.Data
{
    public class VisDbContext : DbContext
    {
      
        public VisDbContext(DbContextOptions<VisDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DocumentRelationModel>().HasKey(table => new {
                table.DoocumentID,
                table.ParentID
            });
        }

        public DbSet<DocumentModel> Document { get; set; }
        public DbSet<DocumentRelationModel> DocumentRelation { get; set; }
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
