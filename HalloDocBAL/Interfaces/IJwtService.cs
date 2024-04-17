using HalloDocDAL.Models;
using System.IdentityModel.Tokens.Jwt;

namespace HalloDocBAL.Interfaces
{
    public interface IJwtService
    {
        public string GenerateToken(Aspnetuser user);
        public bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken);
    }
}
