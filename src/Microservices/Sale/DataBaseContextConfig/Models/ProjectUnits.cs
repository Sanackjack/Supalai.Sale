using Amazon.Runtime.Internal;
using Microsoft.EntityFrameworkCore;
using Twilio.TwiML.Voice;

namespace Spl.Crm.SaleOrder.DataBaseContextConfig.Models;

[Keyless]
public class ProjectUnits
{
    public string? ProjectName  { get; set;} 
    public string? ProjectType { get; set;}
    public string? ProjectID { get; set;}
    public string? UnitID { get; set;}
    public string? Critiria { get; set;}
    public string? HouseNumber { get; set;}
    public string? HousePlanSide { get; set;}
    public string? ModelName { get; set;}
    public string? ModelTypeName { get; set;}
    public string? FurnitureFlag { get; set;}
    public string? Block { get; set;}
    public int? TowerID { get; set;}
    public int? FloorID { get; set;}
    public string? Objective { get; set;}
    public string? AllocatedCode { get; set;}
    public decimal? HouseArea { get; set;}
    public decimal? SellingArea { get; set;}
    public decimal? TitledeedArea { get; set;}
    public decimal? BuildingArea { get; set;}
    public int? AssetType { get; set;}
    public string? UnitStatus { get; set;}
    public string? ShowStatus { get; set;}
    public string? BookingID { get; set;}
    public string? QuotationNumber { get; set;}
    public string? BookingType { get; set;}
    public decimal? SellingPrice { get; set;}
    public decimal? TotalSellingPrice { get; set;}
    public bool? AllowLowPrice { get; set;}
    public decimal? DiscountAmount { get; set;}
    public string? ContractNumber { get; set;}
    public string? SignID { get; set;}
    public string? SaleOrderStatus { get; set;}
    public bool? IsTmp { get; set;}
    public bool? IsQuotation { get; set;}
    public decimal? CR_SellingPrice { get; set;}
    public decimal? CR_TotalSellingPrice { get; set;}
    public decimal? BasePrice { get; set;}
    public decimal? PL_StandardPrice { get; set;}
    public decimal? PL_MarkUpPrice { get; set;}
    public decimal? PL_SellingPrice { get; set;}
    public decimal? PL_Contract2Price { get; set;}
    public decimal? PL_LocationPrice { get; set;}
    public decimal? PL_IncAreaPrice { get; set;}
    public bool? PL_IsHotdeal { get; set;}
    public decimal? PL_DiscountAmount { get; set;}
    public decimal? PL_DecoracteDiscount { get; set;}
    public decimal? PL_DecoracteAmount { get; set;}

    public ProjectUnits()
    {
        this.FurnitureFlag = "N";
    }


}
