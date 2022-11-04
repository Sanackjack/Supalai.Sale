using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;

public interface ISaleOrderRepository
{
    List<SysUserInfo>? FindSysUserInfoRawSqlByUserName(string userName);
}