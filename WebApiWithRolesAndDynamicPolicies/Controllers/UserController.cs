using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiWithRoles.Controllers;

[Authorize(Roles = "User")]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    #region Public methods declaration

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("You have access the User controller");
    }

    #endregion
}