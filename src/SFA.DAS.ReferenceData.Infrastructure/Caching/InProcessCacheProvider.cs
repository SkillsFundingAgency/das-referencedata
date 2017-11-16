using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Infrastructure.Caching
{
    public class InProcessCacheProvider : ICacheProvider
    {
        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult((T)MemoryCache.Default.Get(key));
        }

        public Task SetAsync(string key, object value, TimeSpan slidingExpiration)
        {
            return Task.Run(() =>
            {
                MemoryCache.Default.Set(key, value, new CacheItemPolicy {SlidingExpiration = slidingExpiration});
            });
        }

        public Task SetAsync(string key, object value, DateTimeOffset absoluteExpiration)
        {
            return Task.Run(() =>
            {
                MemoryCache.Default.Set(key, value, new CacheItemPolicy {AbsoluteExpiration = absoluteExpiration});
            });
        }
    }
}