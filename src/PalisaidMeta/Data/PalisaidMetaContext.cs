using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PalisaidMeta.Model
{
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

            Console.WriteLine($"Running in Docker: {AppIn.Docker}");

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
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<TestResultValue> TestResultValuess { get; set; }

        internal IConfiguration? config;
        internal string? connectionString;

        public PalisaidMetaContext()
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("palisaidmetaconfig.json")
               .Build();

            Console.WriteLine($"Running in Docker: {AppIn.Docker}");
            connectionString = config.GetConnectionString(AppIn.Docker ? "docker" : "default");
        }

        public PalisaidMetaContext(DbContextOptions<PalisaidMetaContext> options)
            : base(options) { }

        public PalisaidMetaContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("palisaidmetaconfig.json")
               .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString(AppIn.Docker ? "docker" : "default"));

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

    #endregion context
}