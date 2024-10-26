using System.ComponentModel.DataAnnotations;

namespace WebApiWithRolesAmdDynamicPolicies.DTO.Requests;

public class ChangeUserRoleRequest
{
    #region Public properties declaration

    [Required] [EmailAddress] public string? Email { get; set; }
    [Required] public string? NewRole { get; set; }

    #endregion
}