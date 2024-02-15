using Support.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Authentication.Data
{
    public class DateTimeOffsetConverter : ValueConverter<DateTimeOffset, DateTimeOffset>
    {
        public DateTimeOffsetConverter()
            : base(
                d => d.ToUniversalTime(),
                d => d.ToUniversalTime())
        {
        }
    }
    
    /// <summary>
    /// IdentityDbContext - Supports Azure Identity Management in our PostgreSQL db
    /// </summary>
    public class IdentityDataContext : IdentityDbContext<ApplicationUser>
    {
        internal IConfiguration? config;
        internal string? connectionString;

        public IdentityDataContext()
        {
            config = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json")
              .Build();

            connectionString = config.GetConnectionString("identity");
        }

        public IdentityDataContext(DbContextOptions<IdentityDataContext> options)
            : base(options)
        { }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<DateTimeOffset>()
                .HaveConversion<DateTimeOffsetConverter>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<IdentityDataContext>().HasData();

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            config = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json")
              .Build();

            connectionString = config.GetConnectionString("identity");

            base.OnConfiguring(options);
        }
    }
}