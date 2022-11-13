namespace Spl.Crm.SaleOrder.Cache.Redis.Service;

public interface IRedisCacheService
{
    abstract string Prefix { get; }
    abstract T? Get<T>(string key);
    abstract void Set<T>(string key, T value, int expireTime = 0, int refreshTime = 0);
    abstract void Delete(string key);
    abstract void Refresh(string key);
}

