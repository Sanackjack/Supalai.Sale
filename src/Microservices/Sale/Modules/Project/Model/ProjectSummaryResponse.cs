using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Spl.Crm.SaleOrder.Modules.Project.Model;

public class ProjectSummaryResponse
{

    public int unit_status { get; set; }
    public int summary_unit { get; set; }
}
