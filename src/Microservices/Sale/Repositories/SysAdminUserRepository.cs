using Spl.Crm.SaleOrder.ConfigurationOptions;
using Spl.Crm.SaleOrder.DataBaseContextConfig;
using Spl.Crm.SaleOrder.Entities;

namespace Spl.Crm.SaleOrder.Repositories;

public class SysAdminUserRepository : BaseRepository<SysAdminUser>,ISysAdminUserRepository
{
    public SysAdminUserRepository(SaleOrderDbContext db) : base(db)
    {
    }

    public SysAdminUser FindByUserName(string userName)
    {
        return db.SysAdminUser.Where(x => x.Username == userName).FirstOrDefault();
    }
    
    public SysAdminUser FindByUserId(string userId)
    {
        return db.SysAdminUser.Where(x => x.UserId == userId).FirstOrDefault();
    }
}