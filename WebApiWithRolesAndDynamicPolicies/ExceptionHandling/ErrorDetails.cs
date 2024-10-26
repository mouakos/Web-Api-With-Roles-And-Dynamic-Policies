using System.Text.Json;

namespace WebApiWithRolesAmdDynamicPolicies.ExceptionHandling;

internal class ErrorDetails
{
    #region Public properties declaration

    public string? Message { get; set; }
    public int StatusCode { get; set; }

    #endregion

    #region Public methods declaration

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    #endregion
}