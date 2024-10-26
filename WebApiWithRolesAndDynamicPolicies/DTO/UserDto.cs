using WebApiWithRolesAmdDynamicPolicies.DTO.Requests;

namespace WebApiWithRolesAmdDynamicPolicies.DTO;

public class UserDto : UpdateRequest
{
    #region Public properties declaration

    public string? Email { get; set; }
    public string? Id { get; set; }

    #endregion
}