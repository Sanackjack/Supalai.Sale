using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Caching.Distributed;
using Spl.Crm.SaleOrder.Cache.Redis.Model;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Spl.Crm.SaleOrder.Cache.Redis.Service.implement
{
    public class UserCacheService : RedisCacheService,IUserCacheService
    {

        private static string prefix = "user";
        public UserCacheService(IConfiguration configuration,
                                    IDistributedCache cache) : base(configuration, cache)
        {
        }

        public override T Get<T>(string key)
        {
            string buildKey = string.Join('.', prefix, key);
            var cachedResponse = _cache.Get(buildKey);

            return null == cachedResponse ? default : JsonSerializer.Deserialize<T>(cachedResponse);
        }

        public override void Set<T>(string key, T value)
        {
            string buildKey = string.Join('.', prefix, key);
            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Int32.Parse(redisConfig.UserTimeExpire)),
                SlidingExpiration = TimeSpan.FromMinutes(Int32.Parse(redisConfig.UserSlidingExpire))
            };
            _cache.SetString(buildKey, JsonSerializer.Serialize(value), timeOut);

        }

        public override void Delete(string key)
        {
            string buildKey = string.Join('.', prefix, key);
            _cache.Remove(buildKey);
        }

        public override void Refresh(string key)
        {
            string buildKey = string.Join('.', prefix, key);
            _cache.Refresh(buildKey);
        }

    }
}
