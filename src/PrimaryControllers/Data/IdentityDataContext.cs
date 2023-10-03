using Support.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace Authentication.Data
{
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