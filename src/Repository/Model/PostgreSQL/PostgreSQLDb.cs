/*
 MIT License - PostgreSqlDb.cs

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

using System.Data;
using Hl7Harmonizer.MetaTypes.Model;
using Hl7Harmonizer.Support.Interface;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Hl7Harmonizer.Repository.Model.PostgreSQL
{
    public partial class PostgreSqlDb : IDisposable
    {
        private readonly string? dsn;
        private readonly RepositoryIntent intent;
        private readonly IBaseEventLogger eventLogger = new BaseEventLogger("PostgreSQL");

        internal IConfiguration? config;
        internal byte[]? Partitionid;

        private void GetConfiguration()
        {
            config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();
        }

        public void CheckSetup()
        {
            try
            {
                using (var connection = new NpgsqlConnection(dsn))
                {
                    connection.Open();
                    connection.Close();
                }
            }
            catch
            {
                try
                {
                    // Create Db and Key Tables
                    PostgresTableBuilder.CreateDatabase(intent);
                    PostgresTableBuilder.CreateStoredProcs(intent);
                    PostgresTableBuilder.CreateSettingsTable(intent);
                    PostgresTableBuilder.CreateCredentialsTable(intent);
                    PostgresTableBuilder.CreateLogTable(intent);
                    PostgresTableBuilder.AlterTables(intent);
                    PostgresTableBuilder.RegisterEncryptionKey(intent);

                    // Create Class Tables
                    PostgresTableBuilder.CreateTable<Customer>(new Customer(), intent);
                    PostgresTableBuilder.CreateTable<Patient>(new Patient(), intent);
                    PostgresTableBuilder.CreateTable<Practitioner>(new Practitioner(), intent);
                    PostgresTableBuilder.CreateTable<PatientPractitioner>(new PatientPractitioner(), intent);
                    PostgresTableBuilder.CreateTable<PatientCare>(new PatientCare(), intent);
                    PostgresTableBuilder.CreateTable<Encounter>(new Encounter(), intent);
                    PostgresTableBuilder.CreateTable<Medication>(new Medication(), intent);
                    PostgresTableBuilder.CreateTable<Diagnosis>(new Diagnosis(), intent);
                    PostgresTableBuilder.CreateTable<Prescription>(new Prescription(), intent);
                    PostgresTableBuilder.CreateTable<Treatment>(new Treatment(), intent);

                    // Create Linkage Table
                    PostgresTableBuilder.CreateTable<LookupTable>(new LookupTable(), intent);

                    eventLogger.ReportInfo($"Created databaase and tables");
                }
                catch (Exception ex)
                {
                    throw new Exception($"{eventLogger.ReportError($"Failed to create postgreSql log database on thread {Thread.CurrentThread.ManagedThreadId} : {ex.Message}", false)}");
                }
            }
        }

        public PostgreSqlDb(string dsn, RepositoryIntent intent)
        {
            if (string.IsNullOrEmpty(dsn))
            {
                throw new ArgumentNullException(nameof(dsn));
            }

            this.dsn = dsn;
            this.intent = this.intent;

            CheckSetup();
        }

        public static bool ValidTable(DataSet dataSet, string? tableName = null)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                if (dataSet != null &&
                    dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            else
            {
                if (dataSet != null &&
                    dataSet.Tables.Count > 0 && dataSet?.Tables[tableName]?.Rows.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<int> ExecuteNonQuery(string query, List<KeyValuePair<string, object>>? parameters = null)
        {
            var retval = 0;

            try
            {
                using (var connection = new NpgsqlConnection(dsn))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand())
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }

                        command.Connection = connection;
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                        command.CommandText = query;
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

                        retval = await command.ExecuteNonQueryAsync();
                    }

                    await connection.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(eventLogger.ReportWarning($"ExecuteNonQueryAsync Failed: {ex.Message}"));
            }

            return retval;
        }

        public async Task<IEnumerable<T>?> ExecuteQuery<T>(string? query, List<KeyValuePair<string, object>>? parameters = null) where T : new()
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            var records = new DataSet();

            try
            {
                using (var connection = new NpgsqlConnection(dsn))
                {
                    await connection.OpenAsync();

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }

                        using (var _adapter = new NpgsqlDataAdapter(query, connection))
                        {
                            _adapter.SelectCommand = command;
                            _adapter.Fill(records);
                        }
                    }
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

                    connection?.CloseAsync();

                    if (ValidTable(records))
                    {
                        return await MapDataSetToClassList<T>(records.Tables[0]) as IEnumerable<T>;
                    }
                }
            }
            catch (NpgsqlException px)
            {
                if ("42703" == ((PostgresException)px).SqlState)
                {
                    return default;
                }

                throw new Exception(eventLogger.ReportWarning($"ExecuteQuery({query}) Failed: {px.Message}"));
            }
            catch (Exception ex)
            {
                throw new Exception(eventLogger.ReportError($"Unhandled exception at ExecuteQuery {query} : {ex.Message}"));
            }
            finally
            {
                records?.Dispose();
            }

            return default;
        }

        public async Task<T?> ExecuteScalar<T>(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return default;
            }

            try
            {
                using (var connection = new NpgsqlConnection(dsn))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        var value = await command.ExecuteScalarAsync();
                        if (value != null)
                        {
                            return (T)value;
                        }
                    }
                }
            }
            catch
            {
            }

            return default;
        }

        public async Task<IEnumerable<T>?> ExecuteStoredProceedure<T>(string? procname, List<KeyValuePair<string, object>>? parameterList = null) where T : new()
        {
            if (string.IsNullOrEmpty(procname)) throw new ArgumentNullException(nameof(procname));

            try
            {
                using (var _connection = new NpgsqlConnection(dsn))
                {
                    await _connection.OpenAsync();

                    using (var _proc = new NpgsqlCommand(procname, _connection))
                    {
                        _proc.CommandType = CommandType.StoredProcedure;

                        if (parameterList != null)
                        {
                            foreach (var p in parameterList)
                            {
                                _proc.Parameters.Add(new NpgsqlParameter(p.Key, p.Value));
                            }
                        }

                        using (var records = new DataSet())
                        {
                            using (var _adapter = new NpgsqlDataAdapter())
                            {
                                _adapter.SelectCommand = _proc;
                                _adapter.Fill(records, "_procreturn");
                            }

                            _connection?.CloseAsync();

                            if (ValidTable(records))
                            {
                                return await MapDataSetToClassList<T>(records.Tables[0]) as IEnumerable<T>;
                            }
                        }
                    }
                }

                return default;
            }
            catch (NpgsqlException px)
            {
                if ("42703" == ((PostgresException)px).SqlState)
                {
                    return default;
                }

                throw new Exception(eventLogger.ReportWarning($"Stored proceedure {procname} failed: {px.Message}"));
            }
            catch (Exception ex)
            {
                throw new Exception(eventLogger.ReportError($"Unhandled exception in postgresql adapter executing stored proceedure {procname} : {ex.Message}"));
            }
        }

        #region MapToIEnumerable

        // Convert a DataRow to a T
        public async Task<T?> MapDataSetRow<T>(DataRow row) where T : new()
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            try
            {
                await Task.Run(() =>
                {
                    var item = new T();

                    // Get the PropertyInfo object:
                    var properties = typeof(T).GetProperties().ToList();

                    foreach (var property in properties)
                    {
                        if (property.PropertyType == typeof(System.DayOfWeek))
                        {
                            DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                            property.SetValue(item, day, null);
                        }
                        else
                        {
                            if (row[property.Name] == DBNull.Value)
                            {
                                property.SetValue(item, null, null);
                            }
                            else
                            {
                                if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                                {
                                    object? convertedValue = null;

                                    try
                                    {
                                        convertedValue = System.Convert.ChangeType(row[property.Name],
                                                                Nullable.GetUnderlyingType(property.PropertyType));
                                    }
                                    catch (Exception ex)
                                    {
                                    }

                                    property.SetValue(item, convertedValue, null);
                                }
                                else
                                    property.SetValue(item, row[property.Name], null);
                            }
                        }
                    }

                    return item;
                });
            }
            catch
            {
                throw;
            }

            return default;
        }

        // Convert a DataTable to IEnumerable<T>
        public async Task<IEnumerable<T>> MapDataSetToClassList<T>(DataTable data) where T : new()
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            try
            {
                var itemList = new List<T?>();

                foreach (DataRow row in data.Rows)
                {
                    if (row != null)
                    {
                        itemList.Add(await MapDataSetRow<T>(row));
                    }
                }

                return itemList;
            }
            catch
            {
                throw;
            }
        }

        #endregion MapToIEnumerable

        #region IEnumerableGetRoutines

        public static T NormalizeReturnType<T>(object value)
        {
            return (T)System.Convert.ChangeType(value, typeof(T));
        }

        public async Task<T?> GetById<T>(int id) where T : new()
        {
            return await GetById<T>(id.ToString());
        }

        public async Task<T?> GetById<T>(Guid id) where T : new()
        {
            return await GetById<T>(id.ToString());
        }

        public async Task<T?> GetById<T>(string id) where T : new()
        {
            if (string.IsNullOrEmpty(id))
            {
                return default;
            }

            Type recordType = typeof(T);

            try
            {
                List<KeyValuePair<string?, object?>>? parameters = new()
                {
                    new KeyValuePair<string?, object?>("id","id"),
                    new KeyValuePair<string?, object?>("table","table")
                };

                var list = await ExecuteQuery<T>($"select * from @table where MyId = @id", parameters);
                return list == null
                    ? default
                    : list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                eventLogger.ReportError(ex.Message);
            }

            return default;
        }

        public async Task<T?> GetByName<T>(string name) where T : new()
        {
            if (name == null)
            {
                return default;
            }

            Type recordType = typeof(T);

            try
            {
                throw new NotImplementedException(nameof(GetByName));
            }
            catch (Exception ex)
            {
                eventLogger.ReportError(ex.Message);
            }

            return default;
        }

        public async Task<IEnumerable<T>?> GetListOf<T>(string? query = null, List<KeyValuePair<string, object>>? parameters = null) where T : new()
        {
            var t = typeof(T);

            if (query == null)
            {
                query = $"SELECT * FROM {t.Name}";
            }

            return await ExecuteQuery<T>(query, parameters);
        }

        public async Task<IEnumerable<T>?> GetListOf<T>(LookupResolver owner, Guid ownerid, LookupResolver items) where T : new()
        {
            try
            {
                var parameters = new List<KeyValuePair<string, object>>()
                {
                new KeyValuePair<string, object>("Owner", owner),
                new KeyValuePair<string, object>("Id", ownerid),
                new KeyValuePair<string, object>("Item", items)
                };

                var target = await ExecuteQuery<LookupTable>("SELECT * FROM \"LookupTable\" " +
                    "WHERE ownertype = @Owner AND itemtype = @Item AND  ownerid = @Owner", parameters);

                if (target != null && target.Count() > 0)
                {
                    parameters.Clear();
                    parameters.Add(new KeyValuePair<string, object>("Owner", ownerid));
                    parameters.Add(new KeyValuePair<string, object>("Table", items.ToString()));

                    return await ExecuteQuery<T>("SELECT * FROM @Table " +
                        "WHERE \"OwnerId\" = @Owner", parameters);
                }
            }
            catch (Exception ex)
            {
                eventLogger.ReportError($"Failed locating items: {items.ToString()} owned by {owner.ToString()}-{ownerid.ToString()}");
            }

            return default;
        }

        #endregion IEnumerableGetRoutines

        #region Dispose

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                eventLogger.ReportTrace($"Disposed instance of PostgreSQL on thread: {Thread.CurrentThread.Name}/{Thread.CurrentThread.ManagedThreadId}");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Dispose
    }
}