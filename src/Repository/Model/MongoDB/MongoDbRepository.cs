/*
 MIT License - MongoDbRepository.cs

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

using Hl7Harmonizer.Support.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Hl7Harmonizer.Repository.Model.MongoDB
{
    public class MongoDbRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly string connectstring;
        private string? tenantIdString;
        private Guid tenantId;
        private RepositoryIntent intent;
        private MongoClient? client;
        private IMongoDatabase? database;
        private IMongoCollection<TEntity>? collection;
        private IBaseEventLogger eventLogger = new BaseEventLogger("MongoRepository");
        private DbCache<TEntity> cache = new();

        public RepositoryIntent GetIntent()
        {
            return intent;
        }

        public new DatabaseType GetType()
        {
            return DatabaseType.MongoDb;
        }

        private void GetDatabase(string name)
        {
            try
            {
                client = new MongoClient(connectstring);
                database = client.GetDatabase(name);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(eventLogger.ReportError($"Failed at GetDatabase({name}), {ex.Message}", false));
            }
        }

        private void GetCollection(string name)
        {
            try
            {
                collection = database?.GetCollection<TEntity>(name);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(eventLogger.ReportError($"Failed at GetCollection({name}), {ex.Message}", false));
            }
        }

        public MongoDbRepository(string tenantid, string? connectstring, string? dbname, RepositoryIntent intent)
        {
            if (string.IsNullOrEmpty(dbname))
            {
                throw new ArgumentNullException(eventLogger.ReportError($"Failed to create repository, dbname null", false));
            }

            if (string.IsNullOrEmpty(connectstring))
            {
                throw new ArgumentNullException(eventLogger.ReportError($"Failed to create repository, connectString null", false));
            }

            //BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

            this.connectstring = connectstring;
            SetTenant(tenantid);

            GetDatabase(dbname);
            GetCollection($"{typeof(TEntity).Name}s");
        }

        public MongoDbRepository(Guid tenantid, string? connectstring, string? dbname, RepositoryIntent intent)
        {
            if (string.IsNullOrEmpty(dbname))
            {
                throw new ArgumentNullException(eventLogger.ReportError($"Failed to create repository, dbname null", false));
            }

            if (string.IsNullOrEmpty(connectstring))
            {
                throw new ArgumentNullException(eventLogger.ReportError($"Failed to create repository, connectString null", false));
            }

            this.connectstring = connectstring;
            SetTenant(tenantid);

            GetDatabase(dbname);
            GetCollection($"{typeof(TEntity).Name}s");
        }

        public void SetTenant(string? partid)
        {
            if (string.IsNullOrEmpty(partid))
            {
                throw new ArgumentNullException(eventLogger.ReportError($"Failed to create repository, partition null", false));
            }

            if (partid != Constants.IgnorePartition)
            {
                tenantId = Guid.Parse(partid);
                tenantIdString = partid;
            }
            else
            {
                tenantIdString = Constants.IgnorePartition;
                tenantId = Guid.Empty;
            }
        }

        public void SetTenant(Guid tenantId)
        {
            tenantIdString = tenantId.ToString();
            this.tenantId = tenantId;
        }

        public async Task<bool> Commit()
        {
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<TEntity>?> GetAll(bool filterBools = true)
        {
            try
            {
                if (tenantId == Guid.Empty) // Get everything in the db
                {
                    var builder = Builders<TEntity>.Filter;
                    var filter = filterBools
                        ? builder.Eq("IsDeleted", false)
                          & builder.Eq("IsActive", true)
                        : builder.Ne("EntityID", Guid.Empty);

                    var documents = await collection.Find(filter).ToListAsync();
                    return documents;
                }
                else // Only get the stuff in the defined partition
                {
                    var builder = Builders<TEntity>.Filter;
                    var filter = filterBools
                        ? builder.Eq("Partition", tenantIdString)
                            & builder.Eq("IsDeleted", false)
                            & builder.Eq("IsActive", true)
                        : builder.Eq("TenantId", tenantIdString);

                    var r = await collection!.FindAsync<TEntity>(filter);
                    return r.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<TEntity?> GetByName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var builder = Builders<TEntity>.Filter;
                var filter = (tenantId != Guid.Empty)
                    ? builder.Eq("MetaType", name)
                        & builder.Eq("Partition", tenantIdString)
                        & builder.Eq("IsDeleted", false)
                        & builder.Eq("IsActive", true)
                    : builder.Eq("MetaType", name)
                        & builder.Eq("IsDeleted", false)
                        & builder.Eq("IsActive", true);

                var e = await collection!.FindAsync<TEntity>(filter);
                return e.FirstOrDefault();
            }

            return default;
        }

        public TEntity? GetById(Guid id)
        {
            var found = cache.Check(id);
            if (found != null)
            {
                return found;
            }

            var builder = Builders<TEntity>.Filter;
            var filter = (tenantId != Guid.Empty)
                ? builder.Eq("EntityID", id)
                    & builder.Eq("Partition", tenantIdString)
                    & builder.Eq("IsDeleted", false)
                    & builder.Eq("IsActive", true)
                : builder.Eq("EntityID", id)
                    & builder.Eq("IsDeleted", false)
                    & builder.Eq("IsActive", true);

            var e = collection.Find(filter);
            return cache.Add(e.FirstOrDefault());
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            var found = cache.Check(id);
            if (found != null)
            {
                return found;
            }

            var builder = Builders<TEntity>.Filter;
            var filter = (tenantId != Guid.Empty)
                ? builder.Eq("EntityID", id)
                    & builder.Eq("Partition", tenantIdString)
                    & builder.Eq("IsDeleted", false)
                    & builder.Eq("IsActive", true)
                : builder.Eq("EntityID", id)
                    & builder.Eq("IsDeleted", false)
                    & builder.Eq("IsActive", true);

            var e = await collection.FindAsync<TEntity>(filter);
            return cache.Add(e.FirstOrDefault());
        }

        // Get by the owner id
        public async Task<TEntity?> GetItemByParentId(Guid parent, Guid id)
        {
            var found = cache.Check(id);
            if (found != null)
            {
                return found;
            }

            var builder = Builders<TEntity>.Filter;
            FilterDefinition<TEntity> filter;

            if (tenantId != Guid.Empty)
            {
                filter = builder.Eq("OwnerId", parent)
                    & builder.Eq("EntityId", id)
                    & builder.Eq("Partition", tenantIdString)
                    & builder.Eq("IsDeleted", false)
                    & builder.Eq("IsActive", true);
            }
            else
            {
                filter = builder.Eq("OwnerId", parent)
                    & builder.Eq("EntityID", id)
                    & builder.Eq("IsDeleted", false)
                    & builder.Eq("IsActive", true);
            }

            var e = await collection!.FindAsync<TEntity>(filter);
            return cache.Add(e.FirstOrDefault());
        }

        public async Task<IEnumerable<TEntity>?> GetAllByOwnerId(Guid id)
        {
            var builder = Builders<TEntity>.Filter;
            FilterDefinition<TEntity> filter;

            if ((tenantId != Guid.Empty))
            {
                filter = builder.Eq("OwnerID", id)
                    & builder.Eq("Partition", tenantIdString)
                    & builder.Eq("IsDeleted", false)
                    & builder.Eq("IsActive", true);
            }
            else
            {
                filter = builder.Eq("OwnerID", id)
                    & builder.Eq("IsDeleted", false)
                    & builder.Eq("IsActive", true);
            }

            var e = await collection!.FindAsync<TEntity>(filter);
            return await e.ToListAsync();
        }

        public async Task<bool> CreateRecord(TEntity entity)
        {
            if (string.IsNullOrEmpty(tenantIdString))
            {
                throw new AccessViolationException(eventLogger.ReportError($"Attempt to write to undefined partition"));
            }

            if (entity != null)
            {
                try
                {
                    // TODO:  Filter on EntityID and Upsert, otherwise we get duplicate customers
                    //var exists = await GetByIdAsync(entity.EntityID);
                    var exists = GetById(entity.EntityID);
                    if (exists == null)
                    {
                        entity.Version = 1;
                        collection!.InsertOne(entity);
                        return await Task.FromResult(true);
                    }
                    else
                    {
                        return await UpdateRecord(entity);
                    }
                }
                catch (MongoWriteException wx)
                {
                    // Duplicate key
                    if (wx.WriteError.Code == 11000)
                    {
                        // Can it be updated?
                        var rec = await GetByIdAsync(entity.EntityID);
                        if (rec != null && rec != entity)
                        {
                            return await UpdateRecord(entity);
                        }

                        return await Task.FromResult(true);
                    }
                }
                catch (Exception ex)
                {
                    eventLogger.ReportWarning($"Failed to create record {entity.EntityID}, {ex.Message}");
                }
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> UpdateRecord(TEntity entity)
        {
            try
            {
                if (entity != null)
                {
                    // Update the BSON _id
                    var oldrecord = await GetByIdAsync(entity.EntityID);
                    if (oldrecord != null)
                    {
                        entity.Id = oldrecord.Id;
                        entity.LastUpdate = DateTimeOffset.Now;
                        entity.Version++;
                        var result = await collection.ReplaceOneAsync(eid => eid.EntityID == entity.EntityID, entity);
                        return await Task.FromResult(result.ModifiedCount > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                eventLogger.ReportError($"UpdateRecord: {ex.Message}, EID: {entity.EntityID}", false);
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteRecord(Guid id)
        {
            if (id != Guid.Empty)
            {
                try
                {
                    var builder = Builders<TEntity>.Filter;
                    var filter = builder.Eq("EntityID", id);
                    var update = Builders<TEntity>.Update.Set("IsDeleted", true)
                                                         .Set("LastUpdate", DateTimeOffset.Now);

                    var result = collection!.UpdateOne(filter, update);
                    return await Task.FromResult(result.ModifiedCount > 0);
                }
                catch (Exception ex)
                {
                    eventLogger.ReportError($"DeleteRecord: {ex.Message}, EID: {id}", false);
                }
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> UnDeleteRecord(Guid id)
        {
            if (id != Guid.Empty)
            {
                var entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    var builder = Builders<TEntity>.Filter;
                    var filter = builder.Eq("EntityID", id);
                    var update = Builders<TEntity>.Update.Set("IsDeleted", false);
                    var options = new UpdateOptions { IsUpsert = true };
                    //var result = await collection!.UpdateOneAsync(filter, update, options);
                    var result = collection!.UpdateOne(filter, update, options);
                    return result.ModifiedCount > 0;
                }
            }

            return false;
        }

        /// <summary>
        /// Insert with Upsert enabled
        /// </summary>
        /// <param name="entity"> </param>
        /// <returns> </returns>
        /// <exception cref="InvalidOperationException"> </exception>
        public async Task<bool> Upsert(TEntity entity)
        {
            try
            {
                var builder = Builders<TEntity>.Filter;

                var filter = tenantId != Guid.Empty
                    ? builder.Eq("EntityId", entity.EntityID) & builder.Eq("TenantId", tenantIdString)
                    : builder.Eq("EntityId", entity.EntityID);

                var update = Builders<TEntity>.Update.Inc("Version", entity.Version);
                var options = new UpdateOptions { IsUpsert = true };

                if (filter != null && update != null)
                {
                    //var retval = await collection.UpdateOneAsync(filter, update, options);
                    var retval = await Task.Run(() => collection.UpdateOne(filter, update, options));
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="predicate"> </param>
        /// <returns> List of entities </returns>
        public async Task<IEnumerable<TEntity>?> QueryFluent(Expression<Func<TEntity, bool>> predicate, int limit = -1)
        {
            if (predicate != null)
            {
                if (limit <= 0)
                {
                    return await Task.Run(() => collection.AsQueryable<TEntity>().Where(predicate.Compile()).ToList());
                }
                else
                {
                    return await Task.Run(() => collection.AsQueryable<TEntity>().Where(predicate.Compile()).ToList().Take(limit));
                }
            }

            return default;
        }

        /// <summary>
        /// </summary>
        /// <param name="predicate"> </param>
        /// <returns> List of entities </returns>
        public async Task<IEnumerable<TEntity>?> QueryFluent(string? predicate, int limit = -1)
        {
            if (predicate != null)
            {
                var p = predicate as Expression<Func<TEntity, bool>>;

                if (limit == -1)
                {
                    return await Task.Run(() => collection.AsQueryable<TEntity>().Where(p.Compile()).ToList());
                }
                else
                {
                    return await Task.Run(() => collection.AsQueryable<TEntity>().Where(p.Compile()).ToList().Take(limit));
                }
            }

            return default;
        }

        /// <summary>
        /// <c> QueryGraph </c> Execute query based on GraphQl
        /// </summary>
        /// <param name="query"> </param>
        /// <returns> </returns>
        /// <exception cref="NotImplementedException"> </exception>
        public Task<IEnumerable<TEntity?>>? QueryGraph(string query)
        {
            if (!string.IsNullOrEmpty(query))
            { }

            throw new NotImplementedException();
        }

        public void Dispose()
        {
            database = null;
            collection = null;
            client = null;
            tenantIdString = null;
            tenantId = Guid.Empty;
            GC.SuppressFinalize(this);
        }
    }
}