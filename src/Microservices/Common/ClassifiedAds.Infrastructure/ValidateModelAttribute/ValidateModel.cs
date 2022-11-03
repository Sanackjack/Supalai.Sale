using Microsoft.AspNetCore.Mvc.Filters;

namespace ClassifiedAds.Infrastructure.ValidateModelAttribute;

public class ValidateModel : ActionFilterAttribute
{ 
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new ValidationFailedResult(context.ModelState);
        }
    }
}
