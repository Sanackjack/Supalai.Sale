namespace Spl.Crm.SaleOrder.Cache.Redis.Service;

public interface IMasterConfigCacheService
{
    public void DeleteMasterConfig(string table, string key);
    public T? GetMasterConfig<T>(string table, string key);
}
