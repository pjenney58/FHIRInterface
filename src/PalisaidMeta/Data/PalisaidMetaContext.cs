using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;

namespace PalisaidMeta.Model
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

    public static class AppIn
    {
        public static bool Docker => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_DOCKER_CONTAINER") == "true";

        public static bool Windows => OperatingSystem.IsWindows();
    }

    #region service helper

    public static class IServiceCollectionExtension
    {
        public static IServiceCollection configureservice(this IServiceCollection service, IConfiguration Configuration)
        {
            IConfiguration config = new ConfigurationBuilder()
               .AddJsonFile("palisaidmetaconfig.json")
               .Build();

            Console.WriteLine($"Running in Docker: {AppIn.Docker}");

            service.AddDbContext<PalisaidMetaContext>(options =>
                options.UseNpgsql(config.GetConnectionString(AppIn.Docker ? "docker" : "default")));

            return service;
        }
    }

    #endregion service helper

    #region factory

    public class PalisaidMetaContextFactory : IDesignTimeDbContextFactory<PalisaidMetaContext>
    {
        public PalisaidMetaContextFactory()
        { }

        internal IConfiguration? config;

        public PalisaidMetaContextFactory(IConfiguration configuration)
        {
            config = configuration;
        }

        public PalisaidMetaContext CreateDbContext(string[] args)
        {
            config = new ConfigurationBuilder()
              .AddJsonFile("palisaidmetaconfig.json")
              .Build();

            Debug.WriteLine($"Running in Container: {AppIn.Docker}");

            var optionsBuilder = new DbContextOptionsBuilder<PalisaidMetaContext>();
            optionsBuilder.UseNpgsql(config.GetConnectionString(AppIn.Docker ? "docker" : "default"));

            return new PalisaidMetaContext(optionsBuilder.Options);
        }
    }

    #endregion factory

    #region context

    public class PalisaidMetaContext : DbContext
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
        public DbSet<CollectorConfig> Collectors { get; set; }
        public DbSet<TestResultEntry> TestResults { get; set; }

        internal IConfiguration? config;
        internal string? connectionString;

        public PalisaidMetaContext()
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("palisaidmetaconfig.json")
               .Build();

            Debug.WriteLine($"Running in Container: {AppIn.Docker}");
            connectionString = config.GetConnectionString(AppIn.Docker ? "docker" : "default");
        }

        public PalisaidMetaContext(DbContextOptions<PalisaidMetaContext> options)
            : base(options) { }

        public PalisaidMetaContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<DateTimeOffset>()
                .HaveConversion<DateTimeOffsetConverter>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("palisaidmetaconfig.json")
               .Build();

            // TODO: Remove this when Npgsql fixes the issue with timestamp
            // AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

#if DEBUG
            optionsBuilder.UseNpgsql(config.GetConnectionString(AppIn.Docker ? "docker" : "default"))
                          .EnableSensitiveDataLogging()
                          .EnableDetailedErrors();
#else
            optionsBuilder.UseNpgsql(config.GetConnectionString(AppIn.Docker ? "docker" : "default"));
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Type>();
            modelBuilder.Ignore<CustomAttributeData>();
        }
    }

    #endregion context
}