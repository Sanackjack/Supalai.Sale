using Microsoft.EntityFrameworkCore;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig.Models;

[Keyless]
public class ProjectSummary
{
    public string? ProjectID { get; set; }
    public string UnitStatus { get; set; }
    public int total_unit { get; set; }

    public ProjectSummary()
    {
        this.UnitStatus = "0";
        this.total_unit = 0;
    }
}
