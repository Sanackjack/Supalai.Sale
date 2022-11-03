using System;
using System.Linq;
using ClassifiedAds.CrossCuttingConcerns.BaseResponse;
using ClassifiedAds.CrossCuttingConcerns.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace ClassifiedAds.Infrastructure.ValidateModelAttribute;

public class ValidationResultModel
{
    public StatusResponse status { get; set; }
    public Object? data { get; set; }
    public ValidationResultModel(ModelStateDictionary modelState)
    {
        string msg = string.Empty;
        foreach (var field in modelState.Keys)
        {
            msg = msg + "[" + field + "] -> " + modelState[field] !.Errors.First().ErrorMessage + " ";
        }

        status = new StatusResponse(ResponseData.BAD_REQUEST_CONNECTION.Code,msg);
    }
}
