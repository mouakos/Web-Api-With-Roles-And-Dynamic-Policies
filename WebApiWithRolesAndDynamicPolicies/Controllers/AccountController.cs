using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiWithRolesAmdDynamicPolicies.ActionsFilters;
using WebApiWithRolesAmdDynamicPolicies.DTO.Requests;
using WebApiWithRolesAmdDynamicPolicies.Interfaces;

namespace WebApiWithRolesAmdDynamicPolicies.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(
    IAccountService accountService) : ControllerBase
{
    #region Public methods declaration

    [HttpPost("login")]
    [ServiceFilter(typeof(ModelValidationFilterAttribute))]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var result = await accountService.LoginAsync(model);

        if (result.Succeeded)
            return Ok(new { result.Token });
        return Unauthorized(result.Errors);
    }

    [HttpPost("register")]
    [ServiceFilter(typeof(ModelValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest model)
    {
        var result = await accountService.RegisterAsync(model);

        if (result.Succeeded)
            return Ok(new { message = $"User {model.Email} registered successfully." });
        return BadRequest(result.Errors);
    }

    #endregion
}