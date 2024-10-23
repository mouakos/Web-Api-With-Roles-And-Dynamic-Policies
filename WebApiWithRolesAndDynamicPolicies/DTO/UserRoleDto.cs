using System.ComponentModel.DataAnnotations;

namespace WebApiWithRoles.DTO;

public class UserRoleDto
{
    #region Public properties declaration

    [Required] public string? Role { get; set; }

    [Required, EmailAddress] public string? Email { get; set; }

    #endregion
}