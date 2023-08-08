/*
 MIT License - PostgreSqlRepository.cs

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

namespace Hl7Harmonizer.Repository.Model.PostgreSQL
{
    public class PostgreSqlRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        internal readonly Type type;
        internal string tenantIdString;
        internal Guid tenantId;

        private IBaseEventLogger eventLogger = new BaseEventLogger("NpgRepository");

        internal readonly RepositoryIntent intent;
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> Entities;
        private DbCache<TEntity> cache = new();

        public new DatabaseType GetType()
        {
            return DatabaseType.PostgreSQL;
        }

        public RepositoryIntent GetIntent()
        {
            return intent;
        }

        public PostgreSqlRepository(Guid tenantId, string connectstring)
        {
            type = typeof(TEntity);
            this.tenantId = tenantId;
            this.tenantIdString = tenantId.ToString();

            try
            {
                Context = new NpgDbContext(connectstring);
                Entities = Context.Set<TEntity>();
                eventLogger.ReportInfo($"NpgRepository for type {type.Name} created");
            }
            catch
            {
                eventLogger.ReportInfo($"Failed to create NpgRepository for type {type.Name}");
                throw;
            }
        }

        public async Task<bool> Commit()
        {
            try
            {
                var count = Context.SaveChanges();
                return await Task.FromResult(count > 0);
            }
            catch (Exception ex)
            {
                throw new DbUpdateException(eventLogger.ReportError($"OnCommit: {ex.Message}", false));
            }
        }

        public void Dispose()
        {
            Context.SaveChanges();
            Context.Dispose();
        }

        public async Task<bool> CreateRecord(TEntity entity)
        {
            if (entity != null)
            {
                try
                {
                    if (tenantIdString == Constants.IgnorePartition)
                    {
                    }
                    else
                    {
                        entity.PrepForSave();
                        entity.TenantID = tenantId;
                    }

                    Context.Add(entity);
                    return await Commit();
                }
                catch (NpgsqlException px)
                {
                    ;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.Message.Contains("23505"))
                        {
                            return await UpdateRecord(entity);
                        }
                    }

                    eventLogger.ReportWarning($"Failed to create record {entity.EntityID}, {ex.Message}");
                }
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> UpdateRecord(TEntity entity)
        {
            try
            {
                entity.LastUpdate = DateTimeOffset.UtcNow;
                entity.Version++;

                Entities.Update(entity);
                return await Commit();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("23505"))
                    {
                        return await UpdateRecord(entity);
                    }
                }

                eventLogger.ReportWarning($"Failed to update record {entity.EntityID}, {ex.Message}");
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteRecord(Guid id)
        {
            if (id != Guid.Empty)
            {
                try
                {
                    var r = await Entities.FindAsync(id);
                    if (r != null)
                    {
                        r.IsDeleted = true;
                        r.IsActive = false;
                        r.LastUpdate = DateTimeOffset.UtcNow;

                        Entities.Update(r);
                        return await Commit();
                    }
                }
                catch (Exception ex)
                {
                    eventLogger.ReportWarning($"Failed to delete record {id}, {ex.Message}");
                }
            }

            return await Task.FromResult(false);
        }

        public TEntity? GetById(Guid id)
        {
            if (id != Guid.Empty)
            {
                var found = cache.Check(id);
                if (found != null)
                {
                    return found;
                }

                try
                {
                    return cache.Add(Entities.Find(id));
                }
                catch (Exception ex)
                {
                    eventLogger.ReportWarning($"Failed to find record {id}, {ex.Message}");
                }
            }

            return default;
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                var found = cache.Check(id);
                if (found != null)
                {
                    return found;
                }

                try
                {
                    return cache.Add(await Entities.FindAsync(id));
                }
                catch (Exception ex)
                {
                    eventLogger.ReportWarning($"Failed to find record {id}, {ex.Message}");
                }
            }

            return default;
        }

        public async Task<TEntity?> GetItemByParentId(Guid parent, Guid item)
        {
            try
            {
                var found = cache.Check(item);
                if (found != null)
                {
                    return found;
                }

                if (tenantIdString == Constants.IgnorePartition)
                {
                    return cache.Add(await Entities.FirstOrDefaultAsync(p => p.OwnerID == parent &&
                                                                             p.EntityID == item &&
                                                                             !p.IsDeleted &&
                                                                             p.IsActive));
                }
                else
                {
                    return cache.Add(await Entities.FirstOrDefaultAsync(p => p.Partition == tenantIdString &&
                                                                             p.OwnerID == parent &&
                                                                             p.EntityID == item &&
                                                                             !p.IsDeleted &&
                                                                             p.IsActive));
                }
            }
            catch (Exception ex)
            {
                eventLogger.ReportWarning($"Failed to find record {item}, {ex.Message}");
            }

            return default;
        }

        public async Task<IEnumerable<TEntity>?> GetAllByOwnerId(Guid id)
        {
            try
            {
                if (tenantIdString == Constants.IgnorePartition)
                {
                    return await Entities.Where(p => p.Partition == tenantIdString && p.OwnerID == id
                                                                                   && !p.IsDeleted
                                                                                   && p.IsActive).ToListAsync();
                }
                else
                {
                    return await Entities.Where(p => p.OwnerID == id && p.EntityID == id
                                                                     && !p.IsDeleted
                                                                     && p.IsActive).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                eventLogger.ReportWarning($"Failed to find record {id}, {ex.Message}");
            }

            return default;
        }

        public async Task<IEnumerable<TEntity>?> GetAll(bool filterBools = true)
        {
            try
            {
                if (!filterBools)
                {
                    return tenantIdString != Constants.IgnorePartition
                        ? await Entities.Where(p => p.TenantID == tenantId).ToListAsync()
                        : await Entities.ToListAsync();
                }

                return tenantIdString != Constants.IgnorePartition
                        ? await Entities.Where(d => d.TenantID == tenantId && !d.IsDeleted && d.IsActive).ToListAsync()
                        : await Entities.Where(d => !d.IsDeleted && d.IsActive).ToListAsync();
            }
            catch (Exception ex)
            {
                eventLogger.ReportWarning($"Failed to get all records, {ex.Message}");
            }

            return default;
        }

        /// <summary> </summary> <param name="predicate", type="Expression<Func<TEntity,
        /// bool>>"></param> <returns> List of entities </returns>
        public async Task<IEnumerable<TEntity>?> QueryFluent(Expression<Func<TEntity, bool>> predicate, int limit = -1)
        {
            if (limit <= 0)
            {
                return await Task.Run(() => Entities.Where(predicate.Compile()).ToList());
            }
            else
            {
                return await Task.Run(() => Entities.Where(predicate.Compile()).ToList().Take(limit));
            }
        }

        /// <summary> </summary> <param name="predicate", type="string"></param> <returns> List of
        /// entities </returns>
        public async Task<IEnumerable<TEntity>?> QueryFluent(string predicate, int limit = -1)
        {
            var p = predicate as Expression<Func<TEntity, bool>>;

            if (limit == -1)
            {
                return await Task.Run(() => Entities.Where(p.Compile()).ToList());
            }
            else
            {
                return await Task.Run(() => Entities.Where(p.Compile()).ToList().Take(limit));
            }

            return default;
        }

        public Task<IEnumerable<TEntity>?> QueryGraph(string query)
        {
            throw new NotImplementedException();
        }

        public void SetTenant(string? partid)
        {
            if (string.IsNullOrEmpty(partid))
            {
                throw new ArgumentNullException(eventLogger.ReportError($"Failed to create repository, partition null", false));
            }

            tenantIdString = partid;
            tenantId = Guid.Parse(tenantIdString);
        }

        public void SetTenant(Guid tenantId)
        {
            this.tenantId = tenantId;
            this.tenantIdString = tenantId.ToString();
        }
    }
}