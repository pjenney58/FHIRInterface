using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Collectors.Model;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore;

namespace Collectors.Data
{
    public class TargetDataContext : DbContext
    {
        public DbSet<TargetConfiguration>? Targets;

        internal IConfiguration? config;
        internal string? connectionString;
        internal string? databaseName;

        
        public TargetDataContext()         
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            connectionString = config.GetConnectionString("pgDocker");
        }

        public TargetDataContext(string connectionString)
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
            modelBuilder.Entity<TargetConfiguration>()
                    .HasKey(c => c.TargetId);
        }
    }
}

