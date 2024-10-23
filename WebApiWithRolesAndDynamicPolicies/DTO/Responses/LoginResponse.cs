namespace WebApiWithRoles.DTO.Responses;

public class LoginResponse : GeneralResponse
{
    #region Public properties declaration

    public string Token { get; set; } = string.Empty;

    #endregion
}