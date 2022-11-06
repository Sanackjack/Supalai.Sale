using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.Infrastructure.JWT;
using Spl.Crm.SaleOrder.Modules.Project.Model;

namespace Spl.Crm.SaleOrder.Modules.Project.Service;

public interface IProjectService
{

    public BaseResponse ProjectList(ProjectListRequest projectListRequest);
    public BaseResponse ProjectUnitsList(ProjectUnitsRequest projectUnitsListRequest);

}
