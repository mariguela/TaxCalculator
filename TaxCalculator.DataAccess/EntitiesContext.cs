using Microsoft.EntityFrameworkCore;
using System;
using TaxCalculator.Domain;

namespace TaxCalculator.DataAccess
{
    public class EntitiesContext : DbContext
    {
        public DbSet<Tax> Tax { get; set; }
        public DbSet<TaxTypePostalCodeConfiguration> TaxTypePostalCodeConfiguration { get; set; }

        public EntitiesContext(DbContextOptions<EntitiesContext> options) : base(options)
        {

        }

        public EntitiesContext() : base()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
             .Entity<Tax>()
             .Property(e => e.Type)
             .HasConversion<Int16>();
        }
    }
}
