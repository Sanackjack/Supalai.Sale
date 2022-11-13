using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassifiedAds.Domain.Constants;

namespace ClassifiedAds.Domain.Entities;

[Table("Sys_Master_Projects", Schema = DBConstant.SaleSchema)]
public class SysMasterProjects
{
    [Key]
    public string ProjectID { get; set; }
    public string ProjectName { get; set; }
    public string ProjectType { get; set; }

}
