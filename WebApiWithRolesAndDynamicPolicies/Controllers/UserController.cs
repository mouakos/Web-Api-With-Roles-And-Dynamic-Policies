using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using WebApiWithRolesAmdDynamicPolicies.ActionsFilters;
using WebApiWithRolesAmdDynamicPolicies.DTO.Requests;
using WebApiWithRolesAmdDynamicPolicies.Interfaces;

namespace WebApiWithRolesAmdDynamicPolicies.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "RoleUser")]
public class UserController(
    IAccountService accountService) : ControllerBase
{
    #region Public methods declaration

    [HttpPut("change-user-password")]
    [ServiceFilter(typeof(ModelValidationFilterAttribute))]
    public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordRequest model)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email is null) return Unauthorized(new { message = "User not authenticate." });

        var result = await accountService.ChangePasswordAsync(email, model);
        if (result.Succeeded)
            return Ok(new { message = "Password changed successfully." });
        return BadRequest(result.Errors);
    }

    [HttpDelete("delete-user")]
    public async Task<IActionResult> DeleteUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email is null) return Unauthorized(new { message = "User not authenticate." });

        var result = await accountService.DeleteUserAsync(email);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok(new { message = $"User {email} deleted successfully." });
    }

    [HttpGet("user-info")]
    public async Task<IActionResult> GetUserInfo()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email is null) return Unauthorized(new { message = "User not authenticate." });

        var user = await accountService.GetUserByEmail(email);
        if (user is null) return NotFound(new { message = $"User {email} not found" });
        return Ok(user);
    }

    [HttpPut("update-user-info")]
    [ServiceFilter(typeof(ModelValidationFilterAttribute))]
    public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateRequest model)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email is null) return Unauthorized(new { message = "User not authenticate." });

        var result = await accountService.UpdateUserAsync(email, model);
        if (result.Succeeded)
            return Ok(new { message = "User info updated successfully." });
        return BadRequest(result.Errors);
    }

    #endregion
}