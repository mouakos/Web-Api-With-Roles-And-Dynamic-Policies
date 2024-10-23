using Microsoft.AspNetCore.Identity;
using WebApiWithRoles.DTO;
using WebApiWithRoles.DTO.Responses;
using WebApiWithRoles.Interfaces;
using WebApiWithRoles.JwtFeatures;

namespace WebApiWithRoles.Services;

public class AccountService(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    JwtHandler jwtHandler) : IAccountService
{
    #region Public methods declaration

    /// <inheritdoc />
    public async Task<GeneralResponse> AddRoleAsync(string role)
    {
        if (await roleManager.RoleExistsAsync(role)) return new GeneralResponse { Message = "Role already exists" };
        var result = await roleManager.CreateAsync(new IdentityRole(role));

        return result.Succeeded
            ? new GeneralResponse { Message = "Role added successfully", IsSuccess = true }
            : new GeneralResponse { Message = result.Errors.First().Description };
    }

    /// <inheritdoc />
    public async Task<GeneralResponse> AssignRoleAsync(UserRoleDto userRoleDto)
    {
        var user = await userManager.FindByEmailAsync(userRoleDto.Email!);
        if (user == null) return new GeneralResponse { Message = "User not found" };

        if (!await roleManager.RoleExistsAsync(userRoleDto.Role!))
            return new GeneralResponse { Message = "Invalid role" };

        var result = await userManager.AddToRoleAsync(user, userRoleDto.Role!);

        return result.Succeeded
            ? new GeneralResponse { Message = "Role assigned successfully", IsSuccess = true }
            : new GeneralResponse { Message = result.Errors.First().Description };
    }

    /// <inheritdoc />
    public async Task<LoginResponse> AuthenticateAsync(LoginDto loginDto)
    {
        var response = new LoginResponse();
        var user = await userManager.FindByEmailAsync(loginDto.Email!);

        if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password!))
        {
            response.Message = "Invalid authentication";
            return response;
        }

        var role = await userManager.GetRolesAsync(user);
        var token = jwtHandler.GenerateToken(user, role.ToList());
        response.Message = "Authentication successfully";
        response.Token = token;
        response.IsSuccess = true;
        return response;
    }

    /// <inheritdoc />
    public async Task<GeneralResponse> RegisterAsync(RegisterDto registerDto)
    {
        var user = new IdentityUser { Email = registerDto.Email, UserName = registerDto.Email };
        var result = await userManager.CreateAsync(user, registerDto.Password!);
        return result.Succeeded
            ? new GeneralResponse { Message = "User registered Successfully", IsSuccess = true }
            : new GeneralResponse { Message = result.Errors.First().Description, IsSuccess = false };
    }

    #endregion
}