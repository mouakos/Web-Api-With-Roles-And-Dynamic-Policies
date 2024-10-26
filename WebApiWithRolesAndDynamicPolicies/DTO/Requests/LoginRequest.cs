using System.ComponentModel.DataAnnotations;

namespace WebApiWithRolesAmdDynamicPolicies.DTO.Requests;

public class LoginRequest
{
    #region Public properties declaration

    [Required] [EmailAddress] public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    #endregion
}