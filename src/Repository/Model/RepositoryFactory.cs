/*
 MIT License - RepositoryFactory.cs

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

using Hl7Harmonizer.Repository.Model.CosmosDB;
using Hl7Harmonizer.Repository.Model.MongoDB;
using Hl7Harmonizer.Repository.Model.PostgreSQL;
using Microsoft.Extensions.Configuration;

namespace Hl7Harmonizer.Repository.Model
{
    public static class RepositoryFactory<TEntity> where TEntity : Entity
    {
        private static IConfiguration? config;
        private static Guid TenantId;
        private static RepositoryLocus locus = RepositoryLocus.Container;
        private static RepositoryIntent intent = RepositoryIntent.DataStorage;
        private static string? databaseName = string.Empty;
        private static string? connectString = string.Empty;
        private static DatabaseType databaseType;
        private static readonly IBaseEventLogger eventLogger = new BaseEventLogger("RepositoryFactory");

        private static void GetConfiguration()
        {
            string? connectionKey = string.Empty;
            IConfigurationSection? RepositoryIntent;
            IConfigurationSection? RepositoryInfo;

            config = new ConfigurationBuilder()
               .AddJsonFile("repositorysettings.json")
               .AddEnvironmentVariables()
               .Build();

            if (config == null)
            {
                throw new NullReferenceException(eventLogger.ReportError("Failed constucting config"));
            }

            RepositoryIntent = config?.GetSection("RepositoryIntent");
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
            if (!string.IsNullOrEmpty(connectionKey))
            {
                if (connectionKey.ToUpperInvariant().Contains("MONGO"))
                {
                    databaseType = DatabaseType.MongoDb;
                }
                else if (connectionKey.ToUpperInvariant().Contains("POSTGRES"))
                {
                    databaseType = DatabaseType.PostgreSQL;
                }
                else if (connectionKey.ToUpperInvariant().Contains("COSMOS"))
                {
                    //Thread.Sleep(6000);
                    databaseType = DatabaseType.CosmosDb;
                }
                else
                {
                    databaseType = DatabaseType.Undefined;
                }

                databaseName = RepositoryInfo?.GetValue<string>("DatabaseName");
                connectString = config?.GetConnectionString(connectionKey);
            }
            else
            {
                throw new NullReferenceException("Failed to get connectionKey");
            }
        }

        public static IRepository<TEntity>? GetRepository(string tenantId, RepositoryIntent pintent, RepositoryLocus plocus = RepositoryLocus.NotSpecified)
        {
            if (tenantId != null)
            {
                TenantId = (tenantId == Constants.IgnorePartition)
                        ? Guid.Empty
                        : Guid.Parse(tenantId);
            }

            intent = pintent;
            locus = plocus;

            GetConfiguration();

            switch (databaseType)
            {
                case DatabaseType.MongoDb:
                    return new MongoDbRepository<TEntity>(TenantId, connectString, databaseName, intent);

                case DatabaseType.PostgreSQL:
                    return new PostgreSqlRepository<TEntity>(TenantId, connectString);

                case DatabaseType.CosmosDb:
                    return new CosmosDbRepository<TEntity>(TenantId, connectString, databaseName);

                case DatabaseType.SQLite:
                case DatabaseType.SqlServer:
                case DatabaseType.MySql:

                default:
                    break;
            }

            return default;
        }

        public static IRepository<TEntity>? GetRepository(string? partitionid)
        {
            if (!string.IsNullOrEmpty(partitionid))
            {
                TenantId = Guid.Parse(partitionid);
            }

            GetConfiguration();
            var section = config?.GetSection("CurrentRepository");

            if (section == null)
            {
                throw new NullReferenceException("config.section");
            }

            switch (databaseType)
            {
                case DatabaseType.MongoDb:
                    return new MongoDbRepository<TEntity>(TenantId, connectString, databaseName, intent);

                case DatabaseType.PostgreSQL:
                    return new PostgreSqlRepository<TEntity>(TenantId, connectString);

                case DatabaseType.CosmosDb:
                    return new CosmosDbRepository<TEntity>(TenantId, connectString, databaseName);

                case DatabaseType.SQLite:
                case DatabaseType.SqlServer:
                case DatabaseType.MySql:

                default:
                    break;
            }

            return default;
        }
    }
}