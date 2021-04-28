using Microsoft.IdentityModel.Tokens;
using server.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace server.Services
{
  public class AuthService : IAuthService
  {
    private readonly JwtTokenConfig _jwtTokenConfig;
    private readonly byte[] _secret;
    public AuthService(JwtTokenConfig jwtTokenConfig)
    {
      _jwtTokenConfig = jwtTokenConfig;
      _secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
    }
    public JwtAuthResult GenerateTokens(string username, List<Claim> claims, DateTime now)
    {
      var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
      var jwtToken = new JwtSecurityToken(
          _jwtTokenConfig.Issuer,
          shouldAddAudienceClaim ? _jwtTokenConfig.Audience : string.Empty,
          claims,
          expires: now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
          signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));
      var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

      return new JwtAuthResult
      {
        AccessToken = accessToken,
        Expiration = jwtToken.ValidTo
      };
    }

    public class JwtAuthResult
    {
      public string AccessToken { get; set; }
      public DateTime Expiration { get; set; }
    }
  }
}
