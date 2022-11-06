using ClassifiedAds.Domain.Entities;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;
using Spl.Crm.SaleOrder.Modules.Project.Model;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;

public interface IProjectRepository
{
    public List<SysMasterProjects> FindProjectListRawSql(string? keySearch);

    public List<ProjectUnits> FindProjectUnitListRawSql(ProjectUnitsRequest projectUnitsRequest);


}