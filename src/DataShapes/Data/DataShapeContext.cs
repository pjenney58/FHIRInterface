using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataShapes.Model
{
    #region service helper
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection configureservice(this IServiceCollection service, IConfiguration Configuration)
        {
            IConfiguration config = new ConfigurationBuilder()
               .AddJsonFile("appdataconfig.json")
               .Build();

            service.AddDbContext<DataShapeContext>(options =>
                options.UseNpgsql(config.GetConnectionString("default")));

            return service;
        }
    }
    #endregion

    #region factory
    public class DataShapeContextFactory : IDesignTimeDbContextFactory<DataShapeContext>
    {
        public DataShapeContextFactory() {}

        internal IConfiguration? config;

        public DataShapeContextFactory(IConfiguration configuration)
        {
            config = configuration;
        }

        public DataShapeContext CreateDbContext(string[] args)
        {
             config = new ConfigurationBuilder()
               .AddJsonFile("appdataconfig.json")
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DataShapeContext>();
            optionsBuilder.UseNpgsql(config.GetConnectionString("default"));

            return new DataShapeContext(optionsBuilder.Options);
        }
    }
    #endregion factory

    #region context
    public class DataShapeContext : DbContext
    {
        public DbSet<Tenant>? Tenants { get; set; }
        public DbSet<Patient>? Patients { get; set; }
        public DbSet<Practitioner>? Practitioners { get; set; }
        public DbSet<Location>? Locations { get; set; }
        public DbSet<Encounter>? Encounters { get; set; }
        public DbSet<Diagnosis>? Diagnoses { get; set; }
        public DbSet<Prescription>? Prescriptions { get; set; }
        public DbSet<Treatment>? Treatments { get; set; }
        public DbSet<Observation>? Observations { get; set; }
        public DbSet<Medication>? Medications { get; set; }
        public DbSet<Code>? Codes { get; set; }
        public DbSet<CollectorConfig> Collectors {get; set; }

        internal IConfiguration? config;
        internal string? connectionString;

        public DataShapeContext()
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("appdataconfig.json")
               .Build();

            connectionString = config.GetConnectionString("default");
        }

        public DataShapeContext(DbContextOptions<DataShapeContext> options)
            : base(options) { }

        public DataShapeContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("appdataconfig.json")
               .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("default"));

            // TODO: Unravel the stupid PostgreSQL => DateTimeOffset issue
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Entity>().HasKey(pk => new { pk.EntityId, pk.TenantId });

            modelBuilder.Ignore<Type>();
            modelBuilder.Ignore<CustomAttributeData>();
        }
    }
    #endregion
}

