using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataShapes.Model
{
    public class DataShapeContext : DbContext
    {
        public DbSet<Patient>? Patients;
        public DbSet<Practitioner>? Practitioners;
        public DbSet<Location>? Locations;
        public DbSet<Encounter>? Encounters;
        public DbSet<Diagnosis>? Diagnoses;
        public DbSet<Prescription>? Prescriptions;
        public DbSet<Treatment>? Treatments;
        public DbSet<Observation>? Observations;
        public DbSet<Medication>? Medications;
        public DbSet<Code>? Codes;

        internal IConfiguration? config;
        internal string? connectionString;
        internal string? databaseName;

        
        public DataShapeContext()         
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            connectionString = config.GetConnectionString("pgDocker");
        }

        public DataShapeContext(string connectionString)
        {
            this.connectionString = connectionString;
        }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("pgDocker"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<Entity>()
           //         .HasKey(c => new { c.Id, c.OwnerId, c.TenantId });
        }
    }
}

