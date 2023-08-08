/*
 MIT License - CosmosDbRepository.cs

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

using System.Net;
using Microsoft.Azure.Cosmos.Serialization;

namespace Hl7Harmonizer.Repository.Model.CosmosDB
{
    public class CosmosDbRepository<TEntity> : IRepository<TEntity> where TEntity : Entity, IDisposable
    {
        private readonly string connectstring;
        private readonly string dbname;

        internal readonly Type type;
        internal string tenantIdString;
        internal Guid tenantId;
        internal RepositoryIntent intent;

        private CosmosClient client;
        private Database database;
        private Container container;

        private IBaseEventLogger eventLogger = new BaseEventLogger("CosmosRepository");
        private DbCache<TEntity> cache = new();

        /// <summary>
        /// Generate Id. e.g. "shoppinglist:783dfe25-7ece-4f0b-885e-c0ea72135942"
        /// </summary>
        /// <param name="entity"> </param>
        /// <returns> </returns>
        //public string GenerateId(TEntity entity) => $"{entity.EntityID}";

        /// <summary>
        /// Returns the value of the partition key
        /// </summary>
        /// <param name="entityId"> </param>
        /// <returns> </returns>
        //public PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);

        public new DatabaseType GetType()
        {
            return DatabaseType.CosmosDb;
        }

        public RepositoryIntent GetIntent()
        {
            return intent;
        }

        private async Task SetClient()
        {
            try
            {
                client = await Task.Run(() => new CosmosClient(connectstring));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(eventLogger.ReportError($"Failed at SetClient({type.Name}), {ex.Message}", false));
            }
        }

        private async Task SetDatabase()
        {
            try
            {
                database = await client.CreateDatabaseIfNotExistsAsync(dbname);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(eventLogger.ReportError($"Failed at SetDatabase({type.Name}), {ex.Message}", false));
            }
        }

        private async Task SetContainer()
        {
            try
            {
                /*
                // List of partition keys, in hierarchical order. You can have up to three levels of keys.
                List<string> subpartitionKeyPaths = new List<string>
                {
                    "/TenantId",
                    "/EntityId"
                };

                // Create container properties object
                ContainerProperties containerProperties = new ContainerProperties(
                    id: type.Name,
                    partitionKeyPath: subpartitionKeyPaths
                );
                */
                // Create container - subpartitioned by TenantId -> UserId -> SessionId
                this.container = await database.CreateContainerIfNotExistsAsync(type.Name, "/id");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(eventLogger.ReportError($"Failed at SetContainer({type.Name}), {ex.Message}", false));
            }
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public CosmosDbRepository(Guid tenantId, string connectstring, string dbname)
        {
            this.tenantId = tenantId;
            this.tenantIdString = tenantId.ToString();
            this.connectstring = connectstring;
            this.dbname = dbname;

            type = typeof(TEntity);

            Task.Run(async () =>
            {
                await SetClient();
                await SetDatabase();
                await SetContainer();
            }).Wait();
        }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public async Task<bool> Commit()
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> CreateRecord(TEntity entity)
        {
            try
            {
                /*
                PartitionKey partitionKey = new PartitionKeyBuilder()
                    .Add(entity.TenantID)
                    .Add(entity.EntityID)
                    .Build();
                */

                ItemResponse<TEntity> itemResponse = await container.UpsertItemAsync<TEntity>(entity, new PartitionKey(entity.EntityID.ToString()));
                return await Task.FromResult(true);
            }
            catch (CosmosException ex)
            {
                eventLogger.ReportError(ex.Message, false);
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> UpdateRecord(TEntity entity)
        {
            try
            {
                ItemResponse<TEntity> itemResponse = await container.UpsertItemAsync<TEntity>(entity);
                return await Task.FromResult(true);
            }
            catch (CosmosException ex)
            {
                eventLogger.ReportError(ex.Message, false);
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> DeleteRecord(Guid entityid)
        {
            if (entityid != Guid.Empty)
            {
                try
                {
                    ItemResponse<TEntity> findResponse = await this.container.ReadItemAsync<TEntity>(entityid.ToString(), new PartitionKey(entityid.ToString()));
                    var newEntity = findResponse.Resource;
                    newEntity.IsDeleted = true;
                    newEntity.IsActive = false;
                    newEntity.LastUpdate = DateTimeOffset.Now;
                    ItemResponse<TEntity> upsertResponse = await this.container.ReplaceItemAsync<TEntity>(newEntity, newEntity.EntityID.ToString(), new PartitionKey(newEntity.EntityID.ToString()));
                }
                catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return await Task.FromResult(false);
                }
            }

            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<TEntity>?> GetAll(bool filterBools = true)
        {
            var sqlQueryText = (tenantId == Guid.Empty)
                    ? $"SELECT * FROM c"
                    : $"SELECT * FROM c WHERE c.TenantID = {tenantId}";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<TEntity> queryResultSetIterator = this.container.GetItemQueryIterator<TEntity>(queryDefinition);

            List<TEntity> entities = new List<TEntity>();

            try
            {
                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<TEntity> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (TEntity entity in currentResultSet)
                    {
                        entities.Add(entity);
                    }
                }
            }
            catch (CosmosException ex)
            {
                eventLogger.ReportError($"GetAll():{ex.Message}\n{ex.StackTrace}");
                return default;
            }

            return entities;
        }

        public async Task<IEnumerable<TEntity>?> GetAllByOwnerId(Guid id)
        {
            var sqlQueryText = $"SELECT * FROM c WHERE c.OwnerID = '{id}'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<TEntity> queryResultSetIterator = this.container.GetItemQueryIterator<TEntity>(queryDefinition);

            List<TEntity> entities = new List<TEntity>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<TEntity> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (TEntity entity in currentResultSet)
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public TEntity? GetById(Guid id)
        {
            var found = cache.Check(id);
            if (found != null)
            {
                return found;
            }

            var entity = Task.Run(() => GetByIdAsync(id)).Result;
            return cache.Add(entity);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            var found = cache.Check(id);
            if (found != null)
            {
                return found;
            }

            try
            {
                var sqlQueryText = $"SELECT * FROM c WHERE c.id = '{id}'";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
                FeedIterator<TEntity> queryResultSetIterator = this.container.GetItemQueryIterator<TEntity>(queryDefinition);

                List<TEntity> entities = new List<TEntity>();

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<TEntity> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (TEntity entity in currentResultSet)
                    {
                        entities.Add(entity);
                    }
                }

                return cache.Add(entities.FirstOrDefault());
            }
            catch (CosmosException ex)
            {
                eventLogger.ReportError($"FAIL: GetByIdAsync({id})\n{ex.Message}\n{ex.StackTrace}", false);
                return default;
            }
        }

        public async Task<TEntity?> GetItemByParentId(Guid parent, Guid id)
        {
            var found = cache.Check(id);
            if (found != null)
            {
                return found;
            }

            var sqlQueryText = $"SELECT * FROM c WHERE c.id = '{id}' AND c.OwnerID = '{parent}'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<TEntity> queryResultSetIterator = this.container.GetItemQueryIterator<TEntity>(queryDefinition);

            List<TEntity> entities = new List<TEntity>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<TEntity> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (TEntity entity in currentResultSet)
                {
                    entities.Add(entity);
                }
            }

            return cache.Add(entities.FirstOrDefault());
        }

        public Task<IEnumerable<TEntity>?> QueryFluent(Expression<Func<TEntity, bool>> predicate, int limit = -1)
        {
            if (predicate != null)
            {
                /* TODO: Cosmos LINQ Query
                using FeedIterator<TEntity> setIterator = container.GetItemLinqQueryable<TEntity>();

                //Asynchronous query execution
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        {
                            Console.WriteLine(item.cost);
                        }
                    }

                    if (limit <= 0)
                    {
                    }
                    else
                    {
                    }
                */
            }

            return default;
        }

        public Task<IEnumerable<TEntity>?> QueryFluent(string predicate, int limit = -1)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>?> QueryGraph(string query)
        {
            throw new NotImplementedException();
        }

        public void SetTenant(string? tenantId)
        {
            if (!string.IsNullOrEmpty(tenantId))
            {
                if (tenantId == Constants.IgnorePartition)
                {
                    this.tenantId = Guid.Empty;
                    this.tenantIdString = tenantId.ToString();
                }
                else
                {
                    this.tenantId = Guid.Parse(tenantId);
                    this.tenantIdString = tenantId;
                }
            }
            else
            {
                this.tenantId = Guid.Empty;
            }
        }

        public void SetTenant(Guid tenantId)
        {
            this.tenantId = tenantId;
            this.tenantIdString = tenantId.ToString();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}