﻿using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure;
using Newtonsoft.Json;
using SFA.DAS.ReferenceData.Domain.Interfaces.Caching;
using StackExchange.Redis;

namespace SFA.DAS.ReferenceData.Infrastructure.Caching
{
    public class RedisCache : ICache
    {
        private readonly IDatabase _cache;

        public RedisCache()
        {
            var connectionMultiplexer = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisConnectionString"]);
            _cache = connectionMultiplexer.GetDatabase();
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _cache.KeyExistsAsync(key);
        }

        public async Task<T> GetCustomValueAsync<T>(string key)
        {
            var redisValue = await _cache.StringGetAsync(key);

            return JsonConvert.DeserializeObject<T>(redisValue);
        }

        public async Task SetCustomValueAsync<T>(string key, T customType, int secondsInCache = 300)
        {
            var _lock = new TaskSynchronizationScope();

            await _lock.RunAsync(async () =>
            {
                await _cache.StringSetAsync(key, JsonConvert.SerializeObject(customType), TimeSpan.FromSeconds(secondsInCache));
            });
        }
    }
}