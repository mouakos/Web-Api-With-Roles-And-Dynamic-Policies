using WebApiWithRoles.DTO;
using WebApiWithRoles.DTO.Responses;

namespace WebApiWithRoles.Interfaces;

public interface IAccountService
{
    #region Public methods declaration

    Task<GeneralResponse> AddRoleAsync(string role);
    Task<GeneralResponse> AssignRoleAsync(UserRoleDto userRoleDto);
    Task<LoginResponse> AuthenticateAsync(LoginDto loginDto);
    Task<GeneralResponse> RegisterAsync(RegisterDto registerDto);

    #endregion
}