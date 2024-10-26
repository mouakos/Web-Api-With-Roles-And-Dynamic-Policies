using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApiWithRolesAmdDynamicPolicies.DTO;
using WebApiWithRolesAmdDynamicPolicies.DTO.Requests;
using WebApiWithRolesAmdDynamicPolicies.DTO.Responses;
using WebApiWithRolesAmdDynamicPolicies.Entities;
using WebApiWithRolesAmdDynamicPolicies.Interfaces;
using WebApiWithRolesAmdDynamicPolicies.JwtFeatures;

namespace WebApiWithRolesAmdDynamicPolicies.Services;

public class AccountService(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    JwtHandler jwtHandler) : IAccountService
{
    #region Public methods declaration

    /// <inheritdoc />
    public async Task<BaseResponse> AddRoleAsync(string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
            return BaseResponse.Failure(["Role already exist."]);
        var result = await roleManager.CreateAsync(new IdentityRole { Name = roleName });
        return !result.Succeeded
            ? BaseResponse.Failure(result.Errors.Select(e => e.Description).ToList())
            : BaseResponse.Success();
    }

    /// <inheritdoc />
    public async Task<BaseResponse> ChangePasswordAsync(string email,
        ChangePasswordRequest changePasswordRequestRequest)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null) return BaseResponse.Failure(["User not found"]);
        var result = await userManager.ChangePasswordAsync(user, changePasswordRequestRequest.CurrentPassword!,
            changePasswordRequestRequest.NewPassword!);
        return !result.Succeeded
            ? BaseResponse.Failure(result.Errors.Select(e => e.Description).ToList())
            : BaseResponse.Success();
    }

    /// <inheritdoc />
    public async Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest changeUserRoleRequest)
    {
        var role = await roleManager.FindByNameAsync(changeUserRoleRequest.NewRole!);
        if (role == null) return BaseResponse.Failure([$"Role {changeUserRoleRequest.NewRole} not found."]);
        var user = await userManager.FindByEmailAsync(changeUserRoleRequest.Email!);
        if (user == null)
            return BaseResponse.Failure([$"User {changeUserRoleRequest.Email} not found"]);
        var roles = await userManager.GetRolesAsync(user);
        var removeResult = await userManager.RemoveFromRolesAsync(user, roles);
        if (!removeResult.Succeeded)
            return BaseResponse.Failure(removeResult.Errors.Select(e => e.Description).ToList());
        var addResult = await userManager.AddToRoleAsync(user, changeUserRoleRequest.NewRole!);
        return !addResult.Succeeded
            ? BaseResponse.Failure(addResult.Errors.Select(e => e.Description).ToList())
            : BaseResponse.Success();
    }

    /// <inheritdoc />
    public async Task<BaseResponse> DeleteRoleAsync(string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role is null) return BaseResponse.Failure([$"Role {roleName} does not exist."]);
        var result = await roleManager.DeleteAsync(role);
        return !result.Succeeded
            ? BaseResponse.Failure(result.Errors.Select(e => e.Description).ToList())
            : BaseResponse.Success();
    }

    /// <inheritdoc />
    public async Task<BaseResponse> DeleteUserAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null) return BaseResponse.Failure([$"User {email} not found."]);
        var result = await userManager.DeleteAsync(user);
        return !result.Succeeded
            ? BaseResponse.Failure(result.Errors.Select(e => e.Description).ToList())
            : BaseResponse.Success();
    }

    /// <inheritdoc />
    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        return await roleManager.Roles.Select(r => new RoleDto { RoleName = r.Name, Id = r.Id }).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        return await userManager.Users.Select(u => new UserDto
        {
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Id = u.Id,
            PhoneNumber = u.PhoneNumber
        }).ToListAsync();
    }

    /// <inheritdoc />
    public async Task<UserDto?> GetUserByEmail(string email)
    {
        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser == null) return null;
        var user = new UserDto
        {
            Id = existingUser.Id, Email = existingUser.Email, PhoneNumber = existingUser.PhoneNumber,
            FirstName = existingUser.FirstName, LastName = existingUser.LastName
        };
        return user;
    }

    /// <inheritdoc />
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email!);

        if (user == null || !await userManager.CheckPasswordAsync(user, loginRequest.Password!))
            return LoginResponse.Failure(["Invalid authentication"]);

        var role = await userManager.GetRolesAsync(user);
        var token = jwtHandler.GenerateToken(user, role.ToList());
        return LoginResponse.Success(token);
    }

    /// <inheritdoc />
    public async Task<BaseResponse> RegisterAsync(RegisterRequest registerRequest)
    {
        var existingUser = await userManager.FindByEmailAsync(registerRequest.Email!);
        if (existingUser != null)
            return BaseResponse.Failure([$"Email {registerRequest.Email} already exists."]);
        var user = new ApplicationUser
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Email = registerRequest.Email, UserName = registerRequest.Email,
            PhoneNumber = registerRequest.PhoneNumber
        };

        if (!await roleManager.RoleExistsAsync("User"))
            return BaseResponse.Failure(["User creation failed. Role default role 'User' does not exist."]);

        var result = await userManager.CreateAsync(user, registerRequest.Password!);
        if (!result.Succeeded)
            return BaseResponse.Failure(result.Errors.Select(e => e.Description).ToList());

        var addResult = await userManager.AddToRoleAsync(user, "User");

        if (addResult.Succeeded) return BaseResponse.Success();
        await userManager.DeleteAsync(user);
        return BaseResponse.Failure(["User creation failed."]);
    }

    /// <inheritdoc />
    public async Task<BaseResponse> UpdateUserAsync(string email, UpdateRequest updateRequest)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null) return BaseResponse.Failure([$"User {email} not found."]);
        user.FirstName = updateRequest.FirstName;
        user.LastName = updateRequest.LastName;
        user.PhoneNumber = updateRequest.PhoneNumber;
        var result = await userManager.UpdateAsync(user);
        return !result.Succeeded
            ? BaseResponse.Failure(result.Errors.Select(e => e.Description).ToList())
            : BaseResponse.Success();
    }

    #endregion
}