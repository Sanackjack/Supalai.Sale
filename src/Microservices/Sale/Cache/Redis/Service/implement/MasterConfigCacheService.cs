using Microsoft.Extensions.Caching.Distributed;
using Spl.Crm.SaleOrder.Cache.Redis.Model;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace Spl.Crm.SaleOrder.Cache.Redis.Service.implement
{
    public class MasterConfigCacheService : RedisCacheService,IMasterConfigCacheService
    {

        private static string prefix = "master";

        public MasterConfigCacheService(IConfiguration configuration,  IDistributedCache cache) : base(configuration, cache)
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
            Debug.WriteLine("from MasterConfigCacheServicse.");
            Debug.WriteLine(short.Parse(redisConfig.MasterConfigAbsoluteExpiration));

            string[] configTimeExpi = redisConfig.MasterConfigAbsoluteExpiration.Split(':');
            DateTime date = DateTime.Now.AddDays(1);
            TimeSpan newTime = new TimeSpan(short.Parse(configTimeExpi[0]), short.Parse(configTimeExpi[1]), short.Parse(configTimeExpi[2]));
            date = date.Date + newTime;

            Debug.WriteLine(date);

            string buildKey = string.Join('.', prefix, key);

            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = date,
                SlidingExpiration = TimeSpan.FromMinutes(0)
            };
            _cache.SetString(buildKey, JsonSerializer.Serialize(value), timeOut);
        }


        public T? GetMasterConfig<T>(string table, string key)
        {
            return this.Get<T>(string.Join('.', prefix , table, key));
        }

        public void DeleteMasterConfig(string table, string key)
        {
            this.Delete(string.Join('.',prefix, table, key));
        }
    }


}
