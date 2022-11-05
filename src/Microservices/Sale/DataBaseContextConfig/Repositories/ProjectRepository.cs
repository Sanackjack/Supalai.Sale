using ClassifiedAds.Domain.Entities;
using ClassifiedAds.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;
using System.Reflection.Emit;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;

public class ProjectRepository : BaseRepository<SysMasterProjects>, IProjectRepository
{

    public ProjectRepository(SaleOrderDBContext db) : base(db)
    {

    }

    public List<SysMasterProjects> FindProjectListRawSql(string keySearch)
    {

        string stm = string.Format(@"select DISTINCT SMP.ProjectId, SMP.ProjectName, SMP.ProjectType
                                     from Sys_Master_Projects SMP
                                     left join Sys_Master_ProjectContents SMPC on SMPC.ProjectID = SMP.ProjectID
                                     left join Sys_Master_ProjectAddresses SMPA on SMPA.ProjectID = SMP.ProjectID
                                     left join Sys_Master_ProjectLicenses SMPL on SMPL.ProjectID = SMP.ProjectID
                                     left join Sys_Admin_Bu SAB on SAB.BUCode = SMP.BUID
                                     where SMP.isDelete = 0 {0}",
                                     keySearch is not null ? string.Format($"and SMP.ProjectID = '{keySearch}'") : "");
        return db.SysMasterProjects.FromSqlRaw(stm).ToList();
    }
}