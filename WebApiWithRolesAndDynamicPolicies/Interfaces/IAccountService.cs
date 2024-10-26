using WebApiWithRolesAmdDynamicPolicies.DTO;
using WebApiWithRolesAmdDynamicPolicies.DTO.Requests;
using WebApiWithRolesAmdDynamicPolicies.DTO.Responses;

namespace WebApiWithRolesAmdDynamicPolicies.Interfaces;

public interface IAccountService
{
    #region Public methods declaration

    Task<BaseResponse> AddRoleAsync(string roleName);
    Task<BaseResponse> ChangePasswordAsync(string email, ChangePasswordRequest changePasswordRequestRequest);
    Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest changeUserRoleRequest);
    Task<BaseResponse> DeleteRoleAsync(string roleName);
    Task<BaseResponse> DeleteUserAsync(string email);
    Task<List<RoleDto>> GetAllRolesAsync();
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByEmail(string email);
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    Task<BaseResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<BaseResponse> UpdateUserAsync(string email, UpdateRequest updateRequest);

    #endregion
}