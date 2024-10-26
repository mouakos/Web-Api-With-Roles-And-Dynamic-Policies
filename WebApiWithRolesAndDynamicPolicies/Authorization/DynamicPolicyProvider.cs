using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace WebApiWithRolesAmdDynamicPolicies.Authorization;

public class DynamicPolicyProvider(IOptions<AuthorizationOptions> options) : IAuthorizationPolicyProvider
{
    #region Private properties declaration

    private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; } = new(options);

    #endregion

    #region Public methods declaration

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return BackupPolicyProvider.GetDefaultPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return BackupPolicyProvider.GetFallbackPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith("Role", StringComparison.OrdinalIgnoreCase))
            return BackupPolicyProvider.GetPolicyAsync(policyName);
        var role = policyName["Role".Length..];
        var policy = new AuthorizationPolicyBuilder();
        policy.AddRequirements(new DynamicRoleRequirement(role));
        return Task.FromResult<AuthorizationPolicy?>(policy.Build());
    }

    #endregion
}