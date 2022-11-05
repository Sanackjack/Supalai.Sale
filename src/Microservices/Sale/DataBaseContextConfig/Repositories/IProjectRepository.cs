using ClassifiedAds.Domain.Entities;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Models;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;

public interface IProjectRepository
{
    public List<SysMasterProjects> FindProjectListRawSql(string? keySearch);
}