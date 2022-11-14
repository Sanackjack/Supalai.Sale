using Castle.Components.DictionaryAdapter;
using ClassifiedAds.Infrastructure.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Spl.Crm.SaleOrder.Cache.Redis.Model;
using Spl.Crm.SaleOrder.ConfigurationOptions;
using System.Configuration;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spl.Crm.SaleOrder.Cache.Redis.Service.implement
{
    public abstract class RedisCacheService : IRedisCacheService
    {

        protected readonly IDistributedCache _cache;


        protected RedisConfigModel redisConfig;

        protected RedisCacheService(IConfiguration configuration, IDistributedCache cache)
        {
            _cache = cache;
            redisConfig = configuration.GetSection("Caching:Distributed:Redis:Config").Get<RedisConfigModel>();
        }

        public abstract string Prefix { get; }
        public abstract void Delete(string key);
        public abstract T Get<T>(string key);
        public abstract void Refresh(string key);
        public abstract void Set<T>(string key, T value, int expireTime = 0, int refreshTime = 0);

    }


    public static class RedisCacheExtensions
    {
        public static bool TryGetValue<T>(this IDistributedCache cache, string key, out T? value)
        {
            var val = cache.Get(key);
            value = default;
            if (val == null) return false;
            value = JsonSerializer.Deserialize<T>(val, GetJsonSerializerOptions());
            return true;
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }
    }
}
