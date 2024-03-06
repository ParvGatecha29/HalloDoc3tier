using HalloDocDAL.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDocBAL.Interfaces
{
    public interface IJwtService
    {
        public string GenerateToken(Aspnetuser user);
        public bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken);
    }
}
