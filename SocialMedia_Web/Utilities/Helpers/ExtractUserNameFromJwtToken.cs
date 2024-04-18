using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialMedia_Web.Utilities.Helpers
{
    public class ExtractUserNameFromJwtToken
    {
        public static string GetUserNameFromJwtToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;
            string name = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
            string surnName = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Surname)?.Value;
            return name + " " +surnName;
        }
    }
}
