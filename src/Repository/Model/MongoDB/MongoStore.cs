using Microsoft.Maui.Graphics;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

using System.Xml.Linq;
using HL7Harmonizer.Support.Model;

namespace HL7Harmonizer.Store.Model
{
    public class MongoStore
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        private bool overrideConectString;

        private List<MongoDB.Bson.BsonDocument> dbs = new();
        private IMongoDatabase? database;

        private Connection connection;

        public IEnumerable<MongoDB.Bson.BsonDocument> Databases => connection.client.ListDatabases().ToList();

        public IMongoDatabase? GetDatabase(string? name) => database = connection.client.GetDatabase(name);

        public IMongoCollection<T>? GetCollection<T>(string? name) => database?.GetCollection<T>(name);

        public MongoStore(string? name, string? connectstring = null)
        {
            try
            {
                Name = string.IsNullOrEmpty(name)
                       ? string.Empty
                       : name;

                Id = Guid.NewGuid();

                if (connectstring != null)
                {
                    Name = connectstring;
                    overrideConectString = true;
                }

                connection = new(Name, overrideConectString);

                if (connection == null)
                {
                    throw new InvalidOperationException($"Failed to connect to {name} or {connectstring}");
                }

                database = null;
            }
            catch
            {
                throw;
            }
        }

        private void checkNull(byte[]? id, string? key)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if(string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
        }

        public static T NormalizeReturnType<T>(object value)
        {
            return (T)System.Convert.ChangeType(value, typeof(T));
        }

        public bool Write<T>(byte[] partitionid, T data)  where T : Entity
        {
            try
            {
                if(partitionid == null)
                {
                    throw new ArgumentNullException(nameof(partitionid));
                }

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                Type t = typeof(T);

                var collection = database?.GetCollection<BsonDocument>($"{t.Name}s");
                if (collection == null)
                {
                    throw new NullReferenceException(nameof(collection));
                }

                var p = JsonSerializer.Serialize<T>(data);
                var b = BsonDocument.Parse(p);

                var options = new UpdateOptions()
                {
                    IsUpsert = true
                };

                var builder = Builders<BsonDocument>.Filter;
                var filter = builder.Eq("ObjectId", data.ObjectId) & builder.Eq("PartitionId", partitionid);
                var update = new BsonDocument("$set", b);
                var retval = collection.UpdateOne(filter, update, options);

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public IMongoCollection<BsonDocument> ReadAll<T>()
        {
            Type t = typeof(T);

            var collection = database?.GetCollection<BsonDocument>($"{t.Name}s");
            if (collection == null)
            {
                throw new NullReferenceException(nameof(collection));
            }

            return collection;
        }

        public T ReadById<T>(byte[] partitionid, Guid? key)
        {
            try
            {
                if (partitionid == null)
                {
                    throw new ArgumentNullException(nameof(partitionid));
                }

                if(key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }


                Type t = typeof(T);

                var collection = database?.GetCollection<BsonDocument>($"{t.Name}s");
                if (collection == null)
                {
                    throw new NullReferenceException(nameof(collection));
                }

                var builder = Builders<BsonDocument>.Filter;
                var filter = builder.Eq("PartitionId", partitionid) & builder.Eq("ObjectId", key);
                if (filter != null)
                {
                    var result = collection.Find(filter).ToList();
                    return result.Count > 0 ? JsonSerializer.Deserialize<T>(result.FirstOrDefault().ToJson()) : default;
                }

            }
            catch(Exception ex)
            {
                throw;
            }

            return default;
        }

        public IEnumerable<T> Query<T>(byte[] partitionid, string? key)
        {
            try
            {
                if (partitionid == null)
                {
                    throw new ArgumentNullException(nameof(partitionid));
                }

                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException(nameof(key));
                }


                Type t = typeof(T);

                var collection = database?.GetCollection<BsonDocument>($"{t.Name}s");
                if (collection == null)
                {
                    throw new NullReferenceException(nameof(collection));
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return (IEnumerable<T>)default;
        }
    }
}