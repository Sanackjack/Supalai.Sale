using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Spl.Crm.SaleOrder.Modules.Project.Model;

public class ProjectSummaryRequest
{
    [Required]
    [FromRoute]
    public int projectId { get; set; }

}
