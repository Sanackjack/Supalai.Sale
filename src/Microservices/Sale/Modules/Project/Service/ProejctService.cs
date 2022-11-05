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

}
