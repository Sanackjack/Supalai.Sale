using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ClassifiedAds.Infrastructure.ValidateModelAttribute;

public class ValidationFailedResult : ObjectResult
{
    public ValidationFailedResult(ModelStateDictionary modelState)
        : base(new ValidationResultModel(modelState))
    {
        StatusCode = StatusCodes.Status400BadRequest;
    }
}
