using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Spl.Crm.SaleOrder.Modules.Project.Model;

public class ProjectUnitsResponse
{
    public string criteria { get; set; }

    public string unit_id { get; set; }

    public UnitDetail unit_detail { get; set; }

    public ContractInfo contract_info { get; set; }

    public BookingInfo booking_info { get; set; }

    public PriceListDetail price_list_detail { get; set; }

    public double base_price { get; set; }
}


public class UnitDetail
{
    public string house_number { get; set; } 
    public string house_plan_side { get; set; }
    public string model_name { get; set; }
    public string model_type_name { get; set; }
    public string furniture_flag { get; set; }
    public string block { get; set; }
    public string tower_id { get; set; }
    public string floor_id { get; set; }
    public string objective { get; set; }
    public string allocate_code { get; set; }
    public AreaInfo area_info { get; set; }
    public int? asset_type { get; set; }
    public int? unit_status { get; set; }
    public string show_status { get; set; }

}

public class AreaInfo
{
    public double? house_area { get; set; }
    public double? selling_area { get; set; }
    public double? title_deed_area { get; set; }
    public double? building_area { get; set; }
}

public class ContractInfo
{
    public string contract_number { get; set; }
    public string sale_order_status { get; set; }
    public string sign_id { get; set; }
    public bool? is_tmp { get; set; }
    public bool? is_quotation { get; set; }
    public double? sellling_price { get; set; }
    public double? total_selling_price { get; set; }
}

public class BookingInfo
{
    public string booking_id { get; set; }
    public string quotation_number { get; set; }
    public string booking_type { get; set; }
    public double? selling_price { get; set; }
    public double? total_selling_price { get; set; }
    public bool? allow_low_price { get; set; }
    public double? discount_amount { get; set; }
}

public class PriceListDetail
{
    public double? standard_price { get; set; }
    public double? selling_price { get; set; }
    public double? mark_up_price { get; set; }
    public double? contract_2_price { get; set; }
    public double? location_price { get; set; }
    public double? inc_area_price { get; set; }
    public bool? is_hotdeal { get; set; }
    public double? discount_amount { get; set; }
    public double? decorate_discount { get; set; }
    public double? decorate_amount { get; set; }
}