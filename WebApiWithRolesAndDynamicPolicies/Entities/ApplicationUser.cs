using Microsoft.AspNetCore.Identity;

namespace WebApiWithRolesAmdDynamicPolicies.Entities;

public class ApplicationUser : IdentityUser
{
    #region Public properties declaration

    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    #endregion
}