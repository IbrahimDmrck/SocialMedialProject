using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialMedia_Web.Utilities.Helpers
{
    public class ExtractUserIdentityFromJwtToken
    {
        public static int GetUserIdentityFromJwtToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;
            int userId = Convert.ToInt32(jsonToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value);
            return userId;
        }
    }
}
