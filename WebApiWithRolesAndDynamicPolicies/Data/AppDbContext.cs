using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiWithRolesAmdDynamicPolicies.Entities;

namespace WebApiWithRolesAmdDynamicPolicies.Data;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
{
    #region Protected methods declaration

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed Roles
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
        );

        //Seed Admin Data
        var hasher = new PasswordHasher<ApplicationUser>();
        var adminUser = new ApplicationUser
        {
            FirstName = "trained",
            LastName = "free",
            UserName = "freetrained@freetrained.com",
            NormalizedUserName = "FREETRAINED@FREETRAINED.COM",
            Email = "freetrained@freetrained.com",
            NormalizedEmail = "FREETRAINED@FREETRAINED.COM",
            PhoneNumber = "+4917657754328",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            LockoutEnabled = false
        };

        adminUser.PasswordHash = hasher.HashPassword(adminUser, "freetrained123");

        builder.Entity<ApplicationUser>().HasData(adminUser);

        //Assign Role To Admin
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "1",
                UserId = adminUser.Id
            }
        );
    }

    #endregion
}