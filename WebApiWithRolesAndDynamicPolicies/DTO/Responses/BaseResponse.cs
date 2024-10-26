namespace WebApiWithRolesAmdDynamicPolicies.DTO.Responses;

public class BaseResponse(bool succeeded, List<string>? errors = null)
{
    #region Public properties declaration

    public List<string>? Errors { get; set; } = errors;

    public bool Succeeded { get; set; } = succeeded;

    #endregion

    #region Public methods declaration

    public static BaseResponse Failure(List<string> errors)
    {
        return new BaseResponse(false, errors);
    }

    public static BaseResponse Success()
    {
        return new BaseResponse(true);
    }

    #endregion
}