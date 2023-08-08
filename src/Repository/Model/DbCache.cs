using System;
using Microsoft.Extensions.Caching.Memory;

namespace Hl7Harmonizer.Repository.Model
{
	public class DbCache<TEntity> where TEntity : Entity
	{
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions()
        {
            SizeLimit = 1024
        });

        public TEntity? Check(Guid? id)
        {
            if(id == null || id.Value == Guid.Empty)
            {
                return default;
            }

            TEntity found;

            if (_cache.TryGetValue(id, out found))
            {
                return found;
            }

            return default;
        }

        public TEntity? Check(TEntity? entity)
        {
            if (entity == null)
            {
                return default;
            }

            TEntity found;

            if(_cache.TryGetValue(entity.EntityID, out found))
            {
                return found;
            }
            
            return default;
        }

        public TEntity? Add(TEntity? entity)
        {
            if(entity == null)
            {
                return default;
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1) // Size amount
                .SetPriority(CacheItemPriority.High) // Priority on removing when reaching size limit (memory pressure)
                .SetSlidingExpiration(TimeSpan.FromSeconds(10)) // Keep in cache for this time, reset time if accessed.
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));// Remove from cache after this time, regardless of sliding expiration

            _cache.Set(entity.EntityID, entity, cacheEntryOptions);

            return entity;
        }
    }
}

