using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiWithRoles.Controllers;

//[Authorize(Policy = "AdminPolicy")]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    #region Public methods declaration

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("You have access the Admin controller");
    }

    #endregion
}