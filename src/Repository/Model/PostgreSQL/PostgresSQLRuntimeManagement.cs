/*
 MIT License - PostgreSqlRuntimeManagement.cs

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

using Npgsql;
using Hl7Harmonizer.Support.Interface;
using Microsoft.Extensions.Configuration;

namespace Hl7Harmonizer.Repository.Model.PostgreSQL
{
    public static class PostgresTableBuilder
    {
        private static string? _dsn = string.Empty;
        private static string? _schema = string.Empty;
        private static readonly IBaseEventLogger _eventLogger = new BaseEventLogger(nameof(PostgresTableBuilder));
        internal static IConfiguration? config;

        private static void GetConfiguration()
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();
        }

        private static string? getConnectionString(RepositoryIntent locus, bool admin)
        {
            GetConfiguration();

            var RepositoryIntent = config?.GetSection("RepositoryIntent");
            var RepositoryInfo = RepositoryIntent?.GetSection(locus.ToString());
            var connectionKey = RepositoryInfo?.GetValue<string>("ConnectionKey");

            _schema = RepositoryInfo?.GetValue<string>("DatabaseName");
            _dsn = config?.GetConnectionString(connectionKey);

            /*
            switch (locus)
            {
                case RepositoryIntent.Container:
                    return admin == true
                        ? config?.GetConnectionString("PostgreDockerSqlAdmin")
                        : config?.GetConnectionString("DockerPostgreSql");

                case RepositoryIntent.Remote:
                case RepositoryIntent.Local:
                    return admin == true
                        ? config?.GetConnectionString("PostgreLocalSqlAdmin")
                        : config?.GetConnectionString("LocalPostgreSql");

                default:
                    return default;
            }
            */

            return _dsn;
        }

        // These are the only methods needed to create everything
        private static int ExecuteNonQuery(string command, List<NpgsqlParameter> paramList = null)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_dsn))
                {
                    connection.Open();

                    using (var _command = new NpgsqlCommand())
                    {
                        if (paramList != null)
                        {
                            foreach (var param in paramList)
                            {
                                _command.Parameters.Add(param);
                            }
                        }

                        _command.Connection = connection;
                        _command.CommandText = command;
                        return _command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(_eventLogger.ReportWarning($"{nameof(PostgresTableBuilder)} ExecuteNonQuery Failed: {ex.Message}"));
            }
        }

        private static T ExecuteScalar<T>(string query)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_dsn))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        return (T)command.ExecuteScalar();
                    }
                }
            }
            catch
            {
                return default;
            }
        }

        public static void CreateDatabase(RepositoryIntent locus)
        {
            try
            {
                // Setup as Admin
                GetConfiguration();
                _dsn = getConnectionString(locus, true);

                if (!string.IsNullOrEmpty(_dsn))
                {
                    // As server admin create the database
                    ExecuteNonQuery("CREATE ROLE palisaid WITH LOGIN SUPERUSER INHERIT CREATEDB CREATEROLE REPLICATION PASSWORD '!Palisaid2022';");
                    ExecuteNonQuery("CREATE ROLE palisaiduser WITH LOGIN INHERIT PASSWORD '!Palisaid2022';");
                    ExecuteNonQuery("CREATE DATABASE \"Palisaid\" OWNER=palisaid ENCODING='UTF8' TABLESPACE='pg_default' LC_COLLATE = 'en_US.utf8' LC_CTYPE = 'en_US.utf8' CONNECTION LIMIT=-1");
                }
                else
                {
                    throw new NullReferenceException(nameof(_dsn));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(_eventLogger.ReportError($"Failed creating database: {ex.Message}"));
            }
        }

        public static void CreateSettingsTable(RepositoryIntent locus)
        {
            try
            {
                GetConfiguration();
                _dsn = getConnectionString(locus, false);

                if (!string.IsNullOrEmpty(_dsn))
                {
                    // As database admin create the tables -
                    ExecuteNonQuery("CREATE TABLE public.\"Settings\" (" +
                                "\"Key\" text COLLATE pg_catalog.\"default\", " +
                                "\"Value\" text," +
                                "CONSTRAINT \"PK_public.SettingKeys\" PRIMARY KEY (\"Key\") " +
                                ")");

                    ExecuteNonQuery("ALTER TABLE public.\"Settings\" OWNER to palisaid;");
                }
                else
                {
                    throw new NullReferenceException(nameof(_dsn));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(_eventLogger.ReportError($"Failed creating settings table: {ex.Message}"));
            }
        }

        public static void CreateCredentialsTable(RepositoryIntent locus)
        {
            try
            {
                GetConfiguration();
                _dsn = getConnectionString(locus, false);

                if (!string.IsNullOrEmpty(_dsn))
                {
                    ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS public.\"Credentials\" (" +
                    "\"Id\" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000'::uuid," +
                    "\"Version\" bigint NOT NULL DEFAULT 0," +
                    "\"Username\" text NOT NULL," +
                    "\"Password\" text NOT NULL," +
                    "\"FirstName\" text NOT NULL," +
                    "\"LastName\" text NOT NULL," +
                    "\"CreateDate\" timestamp," +
                    "\"LastUpdate\" timestamp," +
                    "\"IsActive\" bool, " +
                    "\"IsDefault\" bool, " +
                    "CONSTRAINT \"PK_public.CredentialsKeys\" PRIMARY KEY(\"Id\"));");

                    ExecuteNonQuery("ALTER TABLE public.\"Credentials\" OWNER to palisaid;");
                }
                else
                {
                    throw new NullReferenceException(nameof(_dsn));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(_eventLogger.ReportError($"Failed while creating Credentials: {ex.Message}", false));
            }
        }

        public static void AlterTables(RepositoryIntent locus)
        {
            try
            {
                /*
                ExecuteNonQuery("ALTER TABLE IF EXISTS public.\"SynMedConfig\" DROP CONSTRAINT IF EXISTS \"SynMedConfig_Id_key\";");
                ExecuteNonQuery("ALTER TABLE IF EXISTS public.\"SynMedConfig\" DROP CONSTRAINT IF EXISTS \"SynMedConfig_pkey\";");
                ExecuteNonQuery("ALTER TABLE IF EXISTS public.\"SynMedConfig\" DROP CONSTRAINT IF EXISTS \"PK_public.SynMedConfigKeys\";");
                ExecuteNonQuery("ALTER TABLE IF EXISTS public.\"SynMedConfig\" ADD CONSTRAINT \"PK_public.SynMedConfigKeys\" PRIMARY KEY(\"MachineId\");");

                ExecuteNonQuery("ALTER TABLE IF EXISTS public.\"DatabaseConfig\" DROP CONSTRAINT IF EXISTS \"DatabaseConfig_pkey\";");
                ExecuteNonQuery("ALTER TABLE IF EXISTS public.\"DatabaseConfig\" DROP CONSTRAINT IF EXISTS \"PK_public.DatabaseConfigKeys\";");
                ExecuteNonQuery("ALTER TABLE IF EXISTS public.\"DatabaseConfig\" ADD CONSTRAINT \"PK_public.DatabaseConfigKeys\" PRIMARY KEY(\"DatabaseId\");");
                */
            }
            catch (Exception ex)
            {
                throw new Exception(_eventLogger.ReportError($"Failed delete constraint for SynMedConfig table: {ex.Message}", false));
            }
        }

        public static void CreateLogTable(RepositoryIntent locus)
        {
            try
            {
                GetConfiguration();
                _dsn = getConnectionString(locus, false);

                if (!string.IsNullOrEmpty(_dsn))
                {
                    // Create the AuditLog table
                    var sql = "CREATE TABLE IF NOT EXISTS public.\"Logs\" (" +
                          "\"Id\" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000'::uuid," +
                          "\"Version\" bigint NOT NULL DEFAULT 0," +
                          "\"TimeStamp\" timestamp, " +
                          "\"LogName\" text, " +
                          "\"Action\" integer, " +
                          "\"UserId\" uuid, " +
                          "\"Comment\" text, " +
                          "PRIMARY KEY(\"Id\"));";

                    ExecuteNonQuery(sql);
                    ExecuteNonQuery("ALTER TABLE public.\"Logs\" OWNER to palisaid;");
                }
                else
                {
                    throw new NullReferenceException(nameof(_dsn));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(_eventLogger.ReportError($"Failed while creating AuditLog in PostgreSQL: {ex.Message}", false));
            }
        }

        public static void CreateStoredProcs(RepositoryIntent locus)
        {
            /*
            var setlotexp =
                       "CREATE OR REPLACE FUNCTION public.setlotexp(IN rxid uuid,IN cardid uuid, IN lotid text,IN expdate date, IN firstcup integer, IN lastcup integer)\n" +
                       "RETURNS boolean\n" +
                       "LANGUAGE 'plpgsql'\n" +
                       "VOLATILE\n" +
                       "COST 100\n" +
                       "AS $BODY$\n" +
                       "\n--- Update Lot ID and Expiry Date for a drug used on a card\n" +
                       "DECLARE lotrecord uuid;\n" +
                       "DECLARE newguid uuid;\n" +
                       "BEGIN\n" +
                       "SELECT uuid_generate_v4() into newguid;" +
                       "SELECT \"Id\" FROM \"CardFillDetails\" WHERE \"CardId\" = cardid AND \"RxId\" = rxid into lotrecord;\n" +
                       "UPDATE \"Lots\" SET \"Number\" = lotid, \"ExpirationDate\" = expdate, \"FirstCupNumber\" = firstcup, \"LastCupNumber\" = lastcup  WHERE \"CardFillDetailId\" = lotrecord;\n" +
                       "\n" +
                       "--Use special variable FOUND to send the result\n" +
                       "IF FOUND THEN\n" +
                       "    RETURN TRUE;\n" +
                       "ELSE\n" +
                       "    INSERT INTO \"Lots\" (\"Id\",\"CardFillDetailId\",\"Number\",\"ExpirationDate\", \"FirstCupNumber\",\"LastCupNumber\",\"Version\") VALUES (newguid, lotrecord, lotid, expdate, firstcup, lastcup, 1);\n" +
                       "    IF FOUND THEN RETURN TRUE;\n" +
                       "    RETURN FALSE;\n" +
                       "    END IF;" +
                       "END IF;\n" +
                       "END;\n" +
                       "$BODY$;\n";

            ExecuteNonQuery(setlotexp);
            */
        }

        public static void RegisterEncryptionKey(RepositoryIntent locus)
        {
            /*
            // Connect to the motNext database and collect the
            _dsn = ReadSettings["PgConnectString"];
            byte[] key = ExecuteScalar<byte[]>("SELECT \"Key\" FROM \"EncryptionKeys\"");

            // Add the key to the Logger database
            _dsn = ReadSettings["PgLoggerString"];

            List<NpgsqlParameter> paramList = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("guid", Guid.NewGuid()),
                new NpgsqlParameter("bytes", key),
                new NpgsqlParameter("type", "RSA"),
                new NpgsqlParameter("one", 1)
            };

            try
            {
                ExecuteNonQuery($"INSERT INTO \"EncryptionKeys\" VALUES(:guid,:bytes,:type,:one);", paramList);
            }
            catch
            {
                _eventLogger.ReportError("Failed to register encryption key in log database", false);
            }

            // Put the AES Key in here
            */
        }

        /*
         * PostgreSQL type	Default .NET type	Non-default .NET types

            boolean	    bool
            smallint	short	byte, sbyte, int, long, float, double, decimal
            integer	    int	    byte, short, long, float, double, decimal
            bigint	    long	long, byte, short, int, float, double, decimal
            real	    float	double
            double      precision	double
            numeric	    decimal	byte, short, int, long, float, double, BigInteger (6.0+)
            money	    decimal
            text	    string	char[]
            character   varying	string	char[]
            character	string	char[]
            citext	    string	char[]
            json	    string	char[]
            jsonb	    string	char[]
            xml	        string	char[]
            uuid	    Guid
            bytea	    byte[]
            timestamp without time zone	    DateTime (Unspecified)
            timestamp with time zone	    DateTime (Utc1)	DateTimeOffset (Offset=0)2
            date	                        DateTime	DateOnly (6.0+)
            time without time zone	        TimeSpan	TimeOnly (6.0+)
            time with time zone	            DateTimeOffset
            interval	                    TimeSpan3	NpgsqlInterval
            cidr	                        (IPAddress, int)	NpgsqlInet
            inet	                        IPAddress	NpgsqlInet, (IPAddress, int)
            macaddr	                        PhysicalAddress
            tsquery	                        NpgsqlTsQuery
            tsvector	                    NpgsqlTsVector
            bit(1)	                        bool	BitArray
            bit(n)	                        BitArray
            bit                             varying	BitArray
            point	                        NpgsqlPoint
            lseg	                        NpgsqlLSeg
            path	                        NpgsqlPath
            polygon	                        NpgsqlPolygon
            line	                        NpgsqlLine
            circle	                        NpgsqlCircle
            box	                            NpgsqlBox
            hstore	                        Dictionary<string, string>
            oid	                            uint
            xid	                            uint
            cid	                            uint
            oidvector	                    uint[]
            name	                        string	char[]
            (internal) char	                char	byte, short, int, long
            geometry                        (PostGIS)	PostgisGeometry
            record	                        object[]
            composite                       types	T
            range                           types	NpgsqlRange<TElement>
            multirange                      types (PG14)	NpgsqlRange<TElement>[]
            enum                            types	TEnum
            array                           types	Array (of element type)
1 In versions prior to 6.0 (or when Npgsql.EnableLegacyTimestampBehavior is enabled), reading a timestamp with time zone returns a Local DateTime instead of Utc. See the breaking change note for more info.

2 In versions prior to 6.0 (or when Npgsql.EnableLegacyTimestampBehavior is enabled), reading a timestamp with time zone as a DateTimeOffset returns a local offset based on the timezone of the server where Npgsql is running.

3 PostgreSQL intervals with month or year components cannot be read as TimeSpan. Consider using NodaTime's Period type, or NpgsqlInterval.
 */

        public static string getMappedType(System.Reflection.PropertyInfo? prop, bool expand)
        {
            if (prop.PropertyType.IsEnum)
            {
                return "INTEGER";
            }

            switch (prop.PropertyType.Name.ToUpperInvariant())
            {
                case "BOOL":
                case "BOOLEAN":
                    return "BOOLEAN";

                case "BYTE":
                case "SBYTE":
                case "INT16":
                case "SHORT":
                    return "SMALLINT";

                case "INT32":
                case "INT":
                case "INTEGER":
                    return "INTEGER";

                case "INT64":
                case "LONG":
                    return "BIGINT";

                // TODO:  Is there a 128 bit integer type in pg?
                case "LONG LONG":
                    return "BIGINT";

                case "FLOAT":
                    return "REAL";

                case "DOUBLE":
                    return "DOUBLE PERCISION";

                case "DECIMAL":
                    return "Numeric";

                case "STRING":
                    return expand == true
                        ? "TEXT NOT NULL"
                        : "TEXT";

                case "CHAR[]":
                    return "CHARACTER VARYING";

                case "GUID":
                    return expand == true
                        ? "UUID NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000'::uuid"
                        : "UUID";

                case "BYTE[]":
                    return "BYTEA";

                // timestamp without time zone DateTime (Unspecified) timestamp with time zone
                // DateTime(Utc1) DateTimeOffset(Offset = 0)2
                case "DATETIME":
                    return "TIMESTAMP WITHOUT TIME ZONE";

                case "DATETIMEOFFSET":
                    return "TIMESTAMP WITH TIME ZONE";

                // date DateTime DateOnly(6.0 +)
                case "DATEONLY":
                    return "DATE";

                // time without time zone TimeSpan TimeOnly(6.0 +)
                case "TIMESPAN":
                case "TIMEONLY":
                    return "TIME WITHOUT TIME ZONE";

                // time with time zone DateTimeOffset

                // interval TimeSpan3 NpgsqlInterval
                case "TIMESPAN3":
                    return "INTERVAL";

                case "IPADDRESS":
                    return "INET";

                case "PHYSICALADDRESS":
                    return "MACADDR";

                case "BITARRAY":
                    return "BIT(N)";

                case "VARYING":
                    return "BIT";

                case "DICTIONARY<STRING, STRING>":
                    return "HSTORE";

                case "UINT":
                case "UNSIGNED":
                case "UNSIGNED INTEGER":
                    return "OID";

                case "UINT[]":
                case "UNSIGNED[]":
                case "UNSIGNED INTEGER[]":
                    return "OIDVECTOR";

                case "OBJECT[]":
                    return "RECORD";

                case "T":
                case "TYPES":
                    return "COMPOSITE";

                case "ENUM":
                case "TENUM":
                    return "ENUM";
            }

            return "TEXT";
        }

        public static void CreateTable<T>(T record, RepositoryIntent locus)
        {
            try
            {
                var tablespec = $"CREATE TABLE IF NOT EXISTS public.\"{typeof(T).Name}s\" (";
                var properties = typeof(T).GetProperties().ToList();

                foreach (var property in properties)
                {
                    tablespec += $"{property.Name} {getMappedType(property, true)}, ";
                }

                tablespec = tablespec.Substring(0, tablespec.LastIndexOf(','));
                tablespec += ");";

                GetConfiguration();
                _dsn = getConnectionString(locus, false);

                if (!string.IsNullOrEmpty(_dsn))
                {
                    ExecuteNonQuery(tablespec);
                    ExecuteNonQuery($"ALTER TABLE public.\"{typeof(T).Name}s\" OWNER to palisaid;");
                }
                else
                {
                    throw new NullReferenceException(nameof(_dsn));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(_eventLogger.ReportError($"Failed while creating Credentials: {ex.Message}", false));
            }
        }
    }
}