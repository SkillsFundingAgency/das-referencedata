using System;
using System.Threading.Tasks;

namespace SFA.DAS.ReferenceData.Infrastructure.Caching
{
    public interface ICacheProvider
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync(string key, object value, TimeSpan slidingExpiration);
        Task SetAsync(string key, object value, DateTimeOffset absoluteExpiration);
    }
}
