using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiWithRolesAmdDynamicPolicies.ActionsFilters;
using WebApiWithRolesAmdDynamicPolicies.DTO.Requests;
using WebApiWithRolesAmdDynamicPolicies.Interfaces;

namespace WebApiWithRolesAmdDynamicPolicies.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "RoleAdmin")]
public class AdminController(
    IAccountService accountService) : ControllerBase
{
    #region Public methods declaration

    [HttpPost("add-role")]
    public async Task<IActionResult> AddNewRole([FromBody] string roleName)
    {
        if (string.IsNullOrEmpty(roleName))
            return BadRequest(new { message = "Invalid role name" });
        var result = await accountService.AddRoleAsync(roleName);

        if (result.Succeeded)
            return Ok(new { message = $"Role {roleName} added successfully." });
        return BadRequest(result.Errors);
    }

    [HttpPut("change-admin-password")]
    [ServiceFilter(typeof(ModelValidationFilterAttribute))]
    public async Task<IActionResult> ChangeAdminPassword([FromBody] ChangePasswordRequest model)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email is null) return Unauthorized(new { message = "User not authenticate." });

        var result = await accountService.ChangePasswordAsync(email, model);
        if (result.Succeeded)
            return Ok(new { message = "Password changed successfully." });
        return BadRequest(result.Errors);
    }

    [HttpPost("change-user-role")]
    [ServiceFilter(typeof(ModelValidationFilterAttribute))]
    public async Task<IActionResult> ChangeUserRole([FromBody] ChangeUserRoleRequest model)
    {
        var result = await accountService.ChangeUserRoleAsync(model);

        if (result.Succeeded)
            return Ok(new { message = $"User '{model.Email}' role changed to '{model.NewRole}' successfully." });
        return BadRequest(result.Errors);
    }

    [HttpDelete("delete-role")]
    public async Task<IActionResult> DeleteRole([FromBody] string roleName)
    {
        if (string.IsNullOrEmpty(roleName))
            return BadRequest(new { message = "Invalid role name" });
        var result = await accountService.DeleteRoleAsync(roleName);

        if (result.Succeeded)
            return Ok(new { message = $"Role {roleName} deleted successfully." });
        return BadRequest(result.Errors);
    }

    [HttpDelete("delete-user/{email}")]
    public async Task<IActionResult> DeleteUser([FromRoute] string email)
    {
        if (string.IsNullOrEmpty(email)) return BadRequest(new { message = "Invalid email" });
        var result = await accountService.DeleteUserAsync(email);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok(new { message = $"User {email} deleted successfully." });
    }

    [HttpGet("admin-info")]
    public async Task<IActionResult> GetAdminInfo()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email is null) return Unauthorized(new { message = "User not authenticate." });
        var user = await accountService.GetUserByEmail(email);
        if (user is null) return NotFound(new { message = $"User {email} not found" });
        return Ok(user);
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await accountService.GetAllRolesAsync();
        return Ok(roles);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await accountService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPut("update-admin-info")]
    [ServiceFilter(typeof(ModelValidationFilterAttribute))]
    public async Task<IActionResult> UpdateAdminInfo([FromBody] UpdateRequest model)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email is null) return Unauthorized(new { message = "User not authenticate." });

        var result = await accountService.UpdateUserAsync(email, model);
        if (result.Succeeded)
            return Ok(new { message = "Profile updated successfully." });
        return BadRequest(result.Errors);
    }

    #endregion
}