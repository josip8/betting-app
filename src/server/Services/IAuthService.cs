using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static server.Services.AuthService;

namespace server.Services
{
    public interface IAuthService
    {
      public JwtAuthResult GenerateTokens(string username, List<Claim> claims, DateTime now);
    }
}
