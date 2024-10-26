using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiWithRolesAmdDynamicPolicies.ActionsFilters;

public class ModelValidationFilterAttribute : IActionFilter
{
    #region Public methods declaration

    /// <inheritdoc />
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    /// <inheritdoc />
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid) context.Result = new BadRequestObjectResult(context.ModelState);
    }

    #endregion
}