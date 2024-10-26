using System.ComponentModel.DataAnnotations;

namespace WebApiWithRolesAmdDynamicPolicies.DTO.Requests;

public class UpdateRequest
{
    #region Public properties declaration

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [Required] [Phone] public string? PhoneNumber { get; set; }

    #endregion
}