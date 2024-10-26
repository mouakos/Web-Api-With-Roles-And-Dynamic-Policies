using System.ComponentModel.DataAnnotations;

namespace WebApiWithRolesAmdDynamicPolicies.DTO.Requests;

public class RegisterRequest
{
    #region Public properties declaration

    [Required] [EmailAddress] public string? Email { get; set; }

    [Required] [MaxLength(50)] public string? FirstName { get; set; }
    [Required] [MaxLength(50)] public string? LastName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required] [Phone] public string? PhoneNumber { get; set; }

    #endregion
}