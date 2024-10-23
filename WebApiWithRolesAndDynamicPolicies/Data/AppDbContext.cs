using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApiWithRoles.Data;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<IdentityUser>(options)
{
}