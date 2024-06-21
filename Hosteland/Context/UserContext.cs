using BusinessObjects.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace HostelandAuthorization.Context
{
    public class UserContext : IUserContext
    {

        public UserContext()
        {
        }

        public CurrentUserDTO GetCurrentUser(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authorizationHeader != null && authorizationHeader.StartsWith("bearer "))
            {
                var token = authorizationHeader.Substring("bearer ".Length).Trim();
                var tokenHandler = new JwtSecurityTokenHandler();
                var decodedToken = tokenHandler.ReadJwtToken(token);

                var result = new CurrentUserDTO()
                {
                    UserId = decodedToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value,
                    UserName = decodedToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                    Email = decodedToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value,
                    Role = (decodedToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value),
                };
                return result;
            }
            return null;
        }
    }
}
