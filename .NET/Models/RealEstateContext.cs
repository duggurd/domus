using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System.Data;
using Npgsql;

namespace Domus.Models
{
    internal class RealEstateContext : DbContext
    {
        // public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options) 
        //     : base(options)
        // {
        // }

        public DbSet<RealEstate> RealEstates => Set<RealEstate>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Domus.Db.DbConfig.DB_CON_STRING);
        }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new RealEstateTypeConfiguration().Configure(modelBuilder.Entity<RealEstate>());
        }
    }
}