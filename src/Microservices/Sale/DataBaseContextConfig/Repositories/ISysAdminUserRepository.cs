using ClassifiedAds.Domain.Repositories;
using ClassifiedAds.Domain.Entities;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;

public interface ISysAdminUserRepository: IBaseRepository<SysAdminUser>
{
    SysAdminUser FindByUserName(string userName);
    SysAdminUser FindByUserId(string userId);
    void SaveChange();
}