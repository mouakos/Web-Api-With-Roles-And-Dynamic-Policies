using System.ComponentModel.DataAnnotations;

namespace WebApiWithRolesAmdDynamicPolicies.DTO.Requests;

public class ChangePasswordRequest
{
    #region Public properties declaration

    [Required]
    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    #endregion
}