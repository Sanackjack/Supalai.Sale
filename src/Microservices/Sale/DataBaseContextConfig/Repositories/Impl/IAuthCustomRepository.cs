using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories.Impl;

public interface IAuthCustomRepository
{
    List<SysUserInfo>? FindSysUserInfoRawSqlByUserName(string userName);
}