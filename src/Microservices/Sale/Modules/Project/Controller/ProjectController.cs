using ClassifiedAds.Infrastructure.Logging;
using Microsoft.AspNetCore.Mvc;
using Spl.Crm.SaleOrder.ConfigurationOptions;
using ClassifiedAds.CrossCuttingConcerns.Exceptions;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using Spl.Crm.SaleOrder.Modules.Project.Service;
using System.Diagnostics;
using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.Domain.Entities;
using Spl.Crm.SaleOrder.DataBaseContextConfig;
using Spl.Crm.SaleOrder.Modules.Project.Model;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Spl.Crm.SaleOrder.Modules.Project.Controller;

[ApiController]
[Authorize]
public class ProjectController : BaseApiController
{
    private readonly IAppLogger _logger;
    private readonly IProjectService _projectService;
    
    public ProjectController(IAppLogger logger, IProjectService projectService)
    {
        this._logger = logger;
        this._projectService = projectService;
    }

    [HttpGet("projects")]
    public BaseResponse ProjectList( [FromQuery] ProjectListRequest requestModel)
    {
        return _projectService.ProjectList(requestModel);
    }

    [HttpGet("projects/{projectId}/units")]
    public BaseResponse ProjectList([FromRoute][FromQuery] ProjectUnitsRequest requestModel)
    {
        return _projectService.ProjectUnitsList(requestModel);
    }
}
