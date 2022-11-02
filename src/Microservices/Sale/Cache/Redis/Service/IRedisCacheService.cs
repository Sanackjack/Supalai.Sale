namespace Spl.Crm.SaleOrder.Cache.Redis.Service;

public interface IRedisCacheService
{
    abstract T? Get<T>(string key);
    abstract void Set<T>(string key, T value);
    void Delete(string key);
    abstract void Refresh(string key);
}

