using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApiWithRolesAmdDynamicPolicies.Entities;

namespace WebApiWithRolesAmdDynamicPolicies.JwtFeatures;

public class JwtHandler(IConfiguration configuration)
{
    #region Private fields declaration

    private readonly IConfigurationSection m_JwtSettings = configuration.GetRequiredSection("JwtSettings");

    #endregion

    #region Public methods declaration

    public string GenerateToken(ApplicationUser user, List<string> roles)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetClaims(user, roles);
        var securityToken = GenerateJwtSecurityToken(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    #endregion

    #region Private methods declaration

    private static List<Claim> GetClaims(ApplicationUser user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        return claims;
    }

    private JwtSecurityToken GenerateJwtSecurityToken(SigningCredentials signingCredentials, List<Claim> claims)
    {
        return new JwtSecurityToken(
            m_JwtSettings.GetRequiredSection("ValidIssuer").Value,
            expires: DateTime.Now.AddMinutes(double.Parse(m_JwtSettings.GetRequiredSection("ExpiryMinutes").Value!)),
            claims: claims,
            audience: m_JwtSettings.GetRequiredSection("ValidAudience").Value,
            signingCredentials: signingCredentials
        );
    }

    private SigningCredentials GetSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(m_JwtSettings.GetRequiredSection("Key").Value!)),
            SecurityAlgorithms.HmacSha256);
    }

    #endregion
}