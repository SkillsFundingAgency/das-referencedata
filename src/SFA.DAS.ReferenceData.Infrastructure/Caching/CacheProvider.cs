using System;
using System.Threading.Tasks;
using SFA.DAS.ReferenceData.Domain.Interfaces.Caching;

namespace SFA.DAS.ReferenceData.Infrastructure.Caching
{
    public class CacheProvider : ICacheProvider
    {
        private readonly ICache _cache;

        public CacheProvider(ICache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (!await _cache.ExistsAsync(key))
            {
                return default(T);
            }

            return await _cache.GetCustomValueAsync<T>(key);
        }

        public async Task SetAsync(string key, object value, TimeSpan slidingExpiration)
        {
            var cacheSeconds = (int) slidingExpiration.TotalSeconds;
            await _cache.SetCustomValueAsync(key, value, cacheSeconds);
        }

        public async Task SetAsync(string key, object value, DateTimeOffset absoluteExpiration)
        {
            var cacheSeconds = (int)absoluteExpiration.Subtract(DateTimeOffset.Now).TotalSeconds;
            await _cache.SetCustomValueAsync(key, value, cacheSeconds);
        }
    }
}
