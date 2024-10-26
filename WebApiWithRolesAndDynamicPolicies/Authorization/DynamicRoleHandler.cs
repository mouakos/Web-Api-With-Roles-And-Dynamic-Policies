using Microsoft.AspNetCore.Authorization;

namespace WebApiWithRolesAmdDynamicPolicies.Authorization;

public class DynamicRoleHandler : AuthorizationHandler<DynamicRoleRequirement>
{
    #region Protected methods declaration

    /// <inheritdoc />
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        DynamicRoleRequirement requirement)
    {
        if (context.User.IsInRole(requirement.RoleName)) context.Succeed(requirement);

        return Task.CompletedTask;
    }

    #endregion
}

public class DynamicRoleRequirement(string roleName) : IAuthorizationRequirement
{
    #region Public properties declaration

    public string RoleName => roleName;

    #endregion
}