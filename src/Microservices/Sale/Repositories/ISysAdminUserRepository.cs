using Spl.Crm.SaleOrder.Entities;

namespace Spl.Crm.SaleOrder.Repositories;

public interface ISysAdminUserRepository: IBaseRepository<SysAdminUser>
{
    SysAdminUser FindByUserName(string userName);
    SysAdminUser FindByUserId(string userId);
    void SaveChange();
}