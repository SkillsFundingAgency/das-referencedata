using System;
using System.Configuration;
using System.Linq;
using SFA.DAS.ReferenceData.Domain.Interfaces.Caching;
using SFA.DAS.ReferenceData.Infrastructure.Caching;
using StructureMap;
using StructureMap.Pipeline;

namespace SFA.DAS.ReferenceData.Infrastructure.DependencyResolution
{
    public class CachePolicy : ConfiguredInstancePolicy
    {
        protected override void apply(Type pluginType, IConfiguredInstance instance)
        {
            var cacheParameter = instance?.Constructor?.GetParameters().FirstOrDefault(x => x.ParameterType == typeof(ICache));

            if (cacheParameter == null)
                return;

            var cache = GetCache();

            instance.Dependencies.AddForConstructorParameter(cacheParameter, cache);
        }
        
        private static ICache GetCache()
        {
            ICache cache;

            // if (bool.Parse(ConfigurationManager.AppSettings["LocalConfig"]))
            // {
            //     cache = new InMemoryCache();
            // }
            // else
            // {
                cache = new RedisCache();
            //}

            return cache;
        }
    }
}