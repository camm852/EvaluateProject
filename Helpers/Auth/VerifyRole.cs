using EvaluationProjects.Models.Constans;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EvaluationProjects.Helpers.Auth
{
    public static class VerifyRole
    {
        public static bool Verify(JwtSecurityToken token, string roleName)
        {
            var userRole = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return userRole!.ToLower().Equals(roleName);
        }
    }
}
