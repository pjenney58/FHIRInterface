/*
 MIT License - DbContext.cs

Copyright (c) 2021 - Present by Sand Drift Software, LLC
All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using Hl7Harmonizer.MetaTypes.Model;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hl7Harmonizer.Repository.Model.Context
{
    public class NpgContextFactory : IDesignTimeDbContextFactory<NpgDbContext>
    {
        private readonly IBaseEventLogger eventLogger = new BaseEventLogger("PostgreSQLContextFactory");
        internal IConfiguration? config;
        internal string? connectString;
        internal string? databaseName;
        internal RepositoryIntent intent;

        private void GetConfiguration()
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var RepositoryIntent = config?.GetSection("RepositoryIntent");
            var RepositoryInfo = RepositoryIntent?.GetSection("SqlAdmin");
            var RepositoryLocus = RepositoryInfo?.GetSection("Container");
            var connectionKey = RepositoryLocus?.GetValue<string>("ConnectionKey");
            connectString = config?.GetConnectionString("ContainerPostgresAdmin");
        }

        public NpgDbContext CreateDbContext(string[] args)
        {
            try
            {
                GetConfiguration();
                var context = new NpgDbContext(connectString);
                context.Database.Migrate();
                return context;
            }
            catch (Exception ex)
            {
                eventLogger.ReportDebug($"DbContext Factory failed: {ex.Message}");
                throw;
            }
        }
    }

    public class NpgDbContext : DbContext
    {
        private readonly IBaseEventLogger eventLogger = new BaseEventLogger("PostgreSQL");

        internal IConfiguration? config;
        internal string? connectString;
        internal string? databaseName;
        internal bool UseConnectString;

        internal RepositoryIntent intent;
        internal RepositoryLocus locus;

        #region DbSets

        // Updated for Ef7
        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Practitioner> Practitioners => Set<Practitioner>();
        public DbSet<PatientPractitioner> PatientPractitioners => Set<PatientPractitioner>();
        public DbSet<PatientCare> PatientCares => Set<PatientCare>();

        public DbSet<Location> Locations => Set<Location>();

        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<ContactMethod> ContactMethods => Set<ContactMethod>();
        public DbSet<Phone> Phones => Set<Phone>();
        public DbSet<Email> Emails => Set<Email>();
        public DbSet<Identifier> Identifiers => Set<Identifier>();

        public DbSet<Encounter> CareEvents => Set<Encounter>();
        public DbSet<Medication> Medications => Set<Medication>();
        public DbSet<Diagnosis> Diagnoses => Set<Diagnosis>();

        public DbSet<Code> Codes => Set<Code>();
        public DbSet<Observation> Observations => Set<Observation>();
        public DbSet<ObservationItem> ObservationsItem => Set<ObservationItem>();
        public DbSet<PersonName> PersonNames => Set<PersonName>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Prescription> Prescriptions => Set<Prescription>();
        public DbSet<Treatment> Treatments => Set<Treatment>();
        public DbSet<DoseSchedule> DoseSchedules => Set<DoseSchedule>();
        public DbSet<DoseDay> DoseDays => Set<DoseDay>();
        public DbSet<DoseEvent> DoseEvents => Set<DoseEvent>();
        public DbSet<Allergy> Allergies => Set<Allergy>();

        #endregion DbSets

        private void GetConfiguration()
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();
        }

        public void Connect(string connectstring)
        {
            connectString = connectstring;
        }

        public void Connect(RepositoryIntent intent)
        {
            this.intent = intent;
            IConfigurationSection RepositoryInfo;
            string? connectionKey;

            GetConfiguration();

            if (config == null)
            {
                throw new NullReferenceException(eventLogger.ReportError("Failed constucting config"));
            }

            var RepositoryIntent = config?.GetSection("RepositoryIntent");
            if (RepositoryIntent == null)
            {
                throw new NullReferenceException(eventLogger.ReportError("Failed reading config for RepositoryIntent", config.GetValue<bool>("MaximumVerbosity")));
            }

            if (intent == Interface.RepositoryIntent.SqlAdmin || intent == Interface.RepositoryIntent.NoSqlAdmin)
            {
                var AdminSection = RepositoryIntent.GetSection(intent.ToString());
                RepositoryInfo = AdminSection.GetSection(locus.ToString());
            }
            else if (intent == Interface.RepositoryIntent.SqlTesting || intent == Interface.RepositoryIntent.NoSqlTesting)
            {
                var TestingSection = RepositoryIntent.GetSection(intent.ToString());
                RepositoryInfo = TestingSection.GetSection(locus.ToString());
            }
            else
            {
                RepositoryInfo = RepositoryIntent?.GetSection(intent.ToString());
            }

            connectionKey = RepositoryInfo?.GetValue<string>("ConnectionKey");
            databaseName = RepositoryInfo?.GetValue<string>("DatabaseName");
            connectString = config?.GetConnectionString(connectionKey);
        }

        // The connect string has already been disciovered/generated
        public NpgDbContext(string? connectstring)
        {
            if (string.IsNullOrEmpty(connectstring))
            {
                throw new ArgumentNullException(nameof(connectString));
            }

            connectString = connectstring;
            UseConnectString = true;
        }

        public NpgDbContext(RepositoryIntent intent)
        {
            Connect(intent);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            if (!UseConnectString)
            {
                Connect(intent);
            }

            eventLogger.ReportTrace($"Configuring DbContext with {connectString}");

            options.UseNpgsql(connectString);
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            eventLogger.ReportTrace($"Building model DbContext with {connectString}");

            modelBuilder.Entity<Entity>()
                .HasKey(k => new { k.EntityID, k.TenantID });

            /*
            modelBuilder.Entity<Customer>().UseTpcMappingStrategy();

            // About People
            modelBuilder.Entity<Patient>().UseTpcMappingStrategy();
            modelBuilder.Entity<Practitioner>().UseTpcMappingStrategy();
            modelBuilder.Entity<Location>().UseTpcMappingStrategy();

            // About Conditions
            modelBuilder.Entity<Diagnosis>().UseTpcMappingStrategy();
            modelBuilder.Entity<Prescription>().UseTpcMappingStrategy();
            modelBuilder.Entity<Treatment>().UseTpcMappingStrategy();
            modelBuilder.Entity<Encounter>().UseTpcMappingStrategy();
            */
        }
    }
}