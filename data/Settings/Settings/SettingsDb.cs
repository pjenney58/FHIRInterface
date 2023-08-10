namespace Settings
{
    using System.Data.SQLite;
    using System.Reflection;

    public class SettingsDb
    {
        internal readonly string dbname = "settings.db3";

        public SettingsDb()
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                string sql = "CREATE TABLE IF NOT EXISTS settings (key Char(128), value Char(128))";
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }

        internal string GetConnectionString()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "./", ".settings");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return $"Data Source={path}/{dbname}";
        }

        public string GetSetting(string key)
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"SELECT value FROM settings WHERE key = $key";
                command.Parameters.AddWithValue("$key", key);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var value = reader.GetString(0);
                        return value;
                    }
                }

                return default;
            }
        }

        public void PutSetting(string key, string value)
        {
            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"INSERT key,value INTO settings VALUES($key,$value)";

                command.Parameters.AddWithValue("$key", key);
                command.Parameters.AddWithValue("$value", value);
                command.ExecuteNonQuery();
            }
        }
    }
}