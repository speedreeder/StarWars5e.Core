using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StarWars5e.Api.Auth;
using StarWars5e.Api.Helpers;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtIssuerOptions;

        public AuthController(IConfiguration configuration, UserManager<AppUser> userManager, JwtFactory jwtFactory,
            JwtIssuerOptions jwtIssuerOptions)
        {
            _configuration = configuration;
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtIssuerOptions = jwtIssuerOptions;
        }

        [HttpPost]
        public async Task<ActionResult<object>> Refresh(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;
            var localUser = await _userManager.FindByNameAsync(username);
            var savedRefreshToken = localUser.RefreshToken; //retrieve the refresh token from a data store
            if (savedRefreshToken != refreshToken)
                throw new SecurityTokenException("Invalid refresh token.");

            var newJwtToken = await Tokens.GenerateJwtAsync(_jwtFactory.GenerateClaimsIdentity(localUser.UserName),
                _jwtFactory, localUser.UserName, _jwtIssuerOptions,
                new JsonSerializerSettings {Formatting = Formatting.Indented});

            var newRefreshToken = Tokens.GenerateRefreshToken();
            localUser.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(localUser);
            //DeleteRefreshToken(username, refreshToken);
            //SaveRefreshToken(username, newRefreshToken);

            return Ok(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["TokenSigningKey"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token.");

            return principal;
        }
    }
}