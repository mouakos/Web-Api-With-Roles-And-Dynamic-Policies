using System.ComponentModel.DataAnnotations;

namespace WebApiWithRolesAmdDynamicPolicies.DTO.Requests;

public class UserRoleRequest
{
    #region Public properties declaration

    [Required] [EmailAddress] public string? Email { get; set; }

    [Required] public string? Role { get; set; }

    #endregion
}