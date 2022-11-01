using Spl.Crm.SaleOrder.DataBaseContextConfig;
using ClassifiedAds.Domain.Entities;

namespace Spl.Crm.SaleOrder.Repositories;

public class SysAdminRoleRepository: BaseRepository<SysAdminRole>,ISysAdminRoleRepository
{
    public SysAdminRoleRepository(SaleOrderDBContext context) : base(context)
    {
    }
}