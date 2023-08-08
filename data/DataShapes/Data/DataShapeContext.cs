using System;
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
               .AddJsonFile("appsettings.json")
               .Build();

            service.AddDbContext<DataShapeContext>(options =>
                options.UseNpgsql(config.GetConnectionString("pgDocker")));

            return service;
        }
    }
    #endregion

    #region factory
    public class DataShapeContextFactory : IDesignTimeDbContextFactory<DataShapeContext>
    {
        public DataShapeContextFactory() {}

        private IConfiguration config;

        public DataShapeContextFactory(IConfiguration configuration)
        {
            config = configuration;
        }

        public DataShapeContext CreateDbContext(string[] args)
        {
             config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DataShapeContext>();
            optionsBuilder.UseNpgsql(config.GetConnectionString("pgDocker"));

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

        internal IConfiguration? config;
        internal string? connectionString;
        internal string? databaseName;

        public DataShapeContext()
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            connectionString = config.GetConnectionString("pgDocker");
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
               .AddJsonFile("appsettings.json")
               .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("pgDocker"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
    #endregion
}

