using Spl.Crm.SaleOrder.DataBaseContextConfig;
using ClassifiedAds.Domain.Entities;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories.Impl;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;

public class SysAdminRoleRepository: BaseRepository<SysAdminRole>,ISysAdminRoleRepository
{
    public SysAdminRoleRepository(SaleOrderDBContext context) : base(context)
    {
    }
}