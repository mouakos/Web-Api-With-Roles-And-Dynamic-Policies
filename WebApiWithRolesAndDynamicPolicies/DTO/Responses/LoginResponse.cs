namespace WebApiWithRolesAmdDynamicPolicies.DTO.Responses;

public class LoginResponse(bool succeeded, List<string>? errors = null, string? token = null)
    : BaseResponse(succeeded, errors)
{
    #region Public properties declaration

    public string? Token { get; set; } = token;

    #endregion

    #region Public methods declaration

    public new static LoginResponse Failure(List<string> errors)
    {
        return new LoginResponse(false, errors);
    }

    public static LoginResponse Success(string token)
    {
        return new LoginResponse(true, null, token);
    }

    #endregion
}