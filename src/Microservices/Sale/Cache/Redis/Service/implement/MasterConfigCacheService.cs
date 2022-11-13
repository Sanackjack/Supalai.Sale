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

        public override string Prefix => "master";

        public MasterConfigCacheService(IConfiguration configuration,  IDistributedCache cache) : base(configuration, cache)
        {
           
        }

        public override T Get<T>(string key)
        {
            string buildKey = string.Join('.', Prefix, key);
            var cachedResponse = _cache.Get(buildKey);

            return null == cachedResponse ? default : JsonSerializer.Deserialize<T>(cachedResponse);
        }

        public override void Set<T>(string key, T value, int expireTime = 0, int refreshTime = 0)
        {
            /* spcify date for expire */
            //string[] configTimeExpi = redisConfig.MasterConfigAbsoluteExpiration.Split(':');
            //DateTime date = DateTime.Now.AddDays(1);
            //TimeSpan newTime = new TimeSpan(short.Parse(configTimeExpi[0]), short.Parse(configTimeExpi[1]), short.Parse(configTimeExpi[2]));
            //date = date.Date + newTime;
            //var timeOut = new DistributedCacheEntryOptions
            //{
            //    AbsoluteExpiration = date,
            //    SlidingExpiration = TimeSpan.FromMinutes(0)
            //};

            var timeOut = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expireTime == 0 ? int.Parse(redisConfig.MasterConfigTimeExpire) : expireTime),
                SlidingExpiration = TimeSpan.FromMinutes(refreshTime == 0 ? int.Parse(redisConfig.MasterConfigSlidingExpire) : refreshTime)
            };


            string buildKey = string.Join('.', Prefix, key);

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


        public T? GetMasterConfig<T>(string table, string key)
        {
            return this.Get<T>(string.Join('.' , table, key));
        }

        public void DeleteMasterConfig(string table, string key)
        {
            this.Delete(string.Join('.', table, key));
        }
    }


}
