namespace WebApiWithRoles.DTO.Responses;

public class GeneralResponse
{
    #region Public properties declaration

    public bool IsSuccess { get; set; }
    public string? Message { get; set; } = string.Empty;

    #endregion
}