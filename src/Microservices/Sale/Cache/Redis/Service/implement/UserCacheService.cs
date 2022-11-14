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

        public override string Prefix => "user";

        public UserCacheService(IConfiguration configuration,
                                    IDistributedCache cache) : base(configuration, cache)
        {
        }

        public override T Get<T>(string key)
        {
            string buildKey = string.Join('.', Prefix, key);
            var cachedResponse = _cache.Get(buildKey);
            return null == cachedResponse ? default : JsonSerializer.Deserialize<T>(cachedResponse);
        }

        /* 
         * AbsoluteExpirationRelativeToNow - In the absolute expiration you can see that it will expires after one minute whether its accessed or not.
         * SlidingExpiration - While in sliding expiration it will expire cache if cache is not accessed within specified time like one minute.
         */
        public override void Set<T>(string key, T value, int expireTime = 0, int refreshTime = 0)
        {
            string buildKey = string.Join('.', Prefix, key);
            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expireTime == 0 ? int.Parse(redisConfig.UserTimeExpire) : expireTime),
                //SlidingExpiration = TimeSpan.FromMinutes(refreshTime == 0 ? int.Parse(redisConfig.UserSlidingExpire) : refreshTime)
            };
            _cache.SetString(buildKey, JsonSerializer.Serialize(value), timeOut);

        }

        public override void Delete(string key)
        {
            string buildKey = string.Join('.', Prefix, key);
            _cache.Remove(buildKey);
        }

        public override void Refresh(string key)
        {
            string buildKey = string.Join('.', Prefix, key);
            _cache.Refresh(buildKey);
        }

    }
}
