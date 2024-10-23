using System.ComponentModel.DataAnnotations;

namespace WebApiWithRoles.DTO;

public class RegisterDto
{
    #region Public properties declaration

    [Required, EmailAddress] public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    #endregion
}