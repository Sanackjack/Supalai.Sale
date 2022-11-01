using Spl.Crm.SaleOrder.DataBaseContextConfig;
using Spl.Crm.SaleOrder.Entities;

namespace Spl.Crm.SaleOrder.Repositories;

public class SysAdminRoleRepository: BaseRepository<SysAdminRole>,ISysAdminRoleRepository
{
    public SysAdminRoleRepository(SaleOrderDBContext context) : base(context)
    {
    }
}