using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Spl.Crm.SaleOrder.Modules.Project.Model;

public class ProjectUnitsRequest
{
    [Required]
    [FromRoute]
    public int projectId { get; set; }

    [FromQuery]
    public string? searchkey { get; set;}

    [FromQuery]
    public int? num_row { get; set;}

    [FromQuery]
    public int? num_page { get; set; }

    public ProjectUnitsRequest()
    {
        this.num_row = 10;
        this.num_page = 1;
    }
}
