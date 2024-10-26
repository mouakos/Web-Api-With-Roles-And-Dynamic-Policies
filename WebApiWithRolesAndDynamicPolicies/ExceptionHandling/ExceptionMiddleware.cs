using System.Net;

namespace WebApiWithRolesAmdDynamicPolicies.ExceptionHandling;

public class ExceptionMiddleware(RequestDelegate requestDelegate)
{
    #region Public methods declaration

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await requestDelegate(httpContext);
        }
        catch (Exception)
        {
            // Log error
            await HandleExceptionAsync(httpContext);
        }
    }

    #endregion

    #region Private methods declaration

    private static async Task HandleExceptionAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsync(new ErrorDetails
        {
            StatusCode = httpContext.Response.StatusCode,
            Message = "Internal server error."
        }.ToString());
    }

    #endregion
}