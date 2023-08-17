using System.Reflection.Emit;
using DataShapes.Model;
using Microsoft.EntityFrameworkCore;

namespace Collectors.Data
{
    public class CollectorDataContext : DbContext
    {
        internal IConfiguration? config;
        internal string? connectionString;

        public DbSet<CollectorConfig> Configs { get; set; }
        public DbSet<CollectorLogRecord> Logs { get; set; }

        public CollectorDataContext()
        {
            config = new ConfigurationBuilder()
              .AddJsonFile("collectorsupportsettings.json")
              .Build();

            connectionString = config.GetConnectionString("default");
        }

        public CollectorDataContext(DbContextOptions<CollectorDataContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            config = new ConfigurationBuilder()
              .AddJsonFile("collectorsupportsettings.json")
              .Build();

            connectionString = config.GetConnectionString("default");
        
            base.OnConfiguring(options);
        }
    }
}
