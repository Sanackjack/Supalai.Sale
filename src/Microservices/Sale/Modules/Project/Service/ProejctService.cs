using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using ClassifiedAds.CrossCuttingConcerns.Exceptions;
using ClassifiedAds.Domain.Entities;
using ClassifiedAds.Infrastructure.Logging;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Net.Http.Headers;
using Spl.Crm.SaleOrder.DataBaseContextConfig;
using Spl.Crm.SaleOrder.DataBaseContextConfig.Repositories;
using Spl.Crm.SaleOrder.Modules.Project.Model;
using System.Net.Mime;
using static System.Net.Mime.MediaTypeNames;

namespace Spl.Crm.SaleOrder.Modules.Project.Service;

public class ProjectService : IProjectService
{

    private readonly IAppLogger _logger;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IProjectRepository _projectRepository;
    private readonly SaleOrderDBContext _context;

    public ProjectService(IAppLogger logger, 
                            IHttpContextAccessor contextAccessor,
                            IProjectRepository projectRepository,
                            SaleOrderDBContext context)
    {
        this._logger = logger;
        this._contextAccessor = contextAccessor;
        this._projectRepository = projectRepository;
        this._context = context;
    }

    public BaseResponse ProjectList(ProjectListRequest projectListRequest)
    {

        if (projectListRequest.key_search is not null && projectListRequest.key_search.Length <= 3) throw new ValidationErrorException(ResponseData.VALIDATION_REQUEST_PARAMETER_FAIL);

        List<ProjectListResponse> projects = new List<ProjectListResponse>();
        foreach (var item in _projectRepository.FindProjectListRawSql(projectListRequest.key_search))
        {
            projects.Add( new ProjectListResponse()
            {
                project_id = item.ProjectID,
                project_name = item.ProjectName,
                project_type = item.ProjectType
            });
        }
        return new BaseResponse(new StatusResponse(), new { projects = projects } );
    }


    public BaseResponse ProjectUnitsList(ProjectUnitsRequest projectUnitsListRequest)
    {
        if (projectUnitsListRequest.searchkey is not null && projectUnitsListRequest.searchkey.Length <= 3) throw new ValidationErrorException(ResponseData.VALIDATION_REQUEST_PARAMETER_FAIL);

        var respnoseModel = _projectRepository.FindProjectUnitListRawSql(projectUnitsListRequest).Select(m => new ProjectUnitsResponse()
                            {
                                criteria = m.Critiria,
                                unit_id = m.UnitID,
                                unit_detail = new UnitDetail()
                                {
                                    house_number = m.HouseNumber,
                                    house_plan_side = m.HousePlanSide,
                                    model_name = m.ModelName,
                                    model_type_name = m.ModelTypeName,
                                    furniture_flag = m.FurnitureFlag,
                                    block = m.Block,
                                    tower_id = m.TowerID?.ToString(),
                                    floor_id = m.FloorID?.ToString(),
                                    objective = m.Objective,
                                    allocate_code = m.AllocatedCode,
                                    area_info = new AreaInfo()
                                    {
                                       house_area = (double?) m.HouseArea,
                                       selling_area = (double?) m.SellingArea,
                                       title_deed_area = (double?) m.TitledeedArea,
                                       building_area = (double?) m.BuildingArea
                                    },
                                    asset_type = m.AssetType,
                                    unit_status = Int32.Parse( m.UnitStatus ),
                                    show_status = m.ShowStatus
                                },
                                contract_info = new ContractInfo()
                                {
                                    contract_number = m.ContractNumber,
                                    sale_order_status = m.SaleOrderStatus,
                                    sign_id = m.SignID,
                                    is_tmp = m.IsTmp,
                                    is_quotation = m.IsQuotation,
                                    sellling_price = (double?) m.SellingPrice,
                                    total_selling_price = (double?) m.TotalSellingPrice
                                },
                                booking_info = new BookingInfo()
                                {
                                    booking_id = m.BookingID,
                                    quotation_number = m.QuotationNumber,
                                    booking_type = m.BookingType,
                                    selling_price = (double?) m.SellingPrice,
                                    total_selling_price = (double?) m.TotalSellingPrice,
                                    allow_low_price = m.AllowLowPrice,
                                    discount_amount = (double?) m.DiscountAmount
                                },
                                price_list_detail = new PriceListDetail()
                                {
                                    standard_price = (double?) m.PL_StandardPrice,
                                    selling_price = (double?) m.PL_SellingPrice,
                                    mark_up_price = (double?) m.PL_MarkUpPrice,
                                    contract_2_price = (double?) m.PL_Contract2Price,
                                    location_price = (double?) m.PL_LocationPrice,
                                    inc_area_price = (double?) m.PL_IncAreaPrice,
                                    is_hotdeal = m.PL_IsHotdeal,
                                    discount_amount = (double?)m.PL_DiscountAmount,
                                    decorate_discount = (double?)m.PL_DecoracteDiscount,
                                    decorate_amount = (double?)m.PL_DecoracteAmount
                                }

                            }).ToList();

        return new BaseResponse(new StatusResponse(), respnoseModel);
    }
}
