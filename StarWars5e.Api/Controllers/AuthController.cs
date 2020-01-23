using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
            IOptions<JwtIssuerOptions> jwtIssuerOptions)
        {
            _configuration = configuration;
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtIssuerOptions = jwtIssuerOptions.Value;
        }

        [HttpGet("refresh")]
        public async Task<ActionResult<object>> RefreshAsync()
        {
            var token = Request.Cookies["sw5e_accessToken"];
            var refreshToken = Request.Cookies["sw5e_refreshToken"];

            var principal = GetPrincipalFromToken(token);
            var userId = principal.FindFirst(c => c.Type == Constants.Strings.JwtClaimIdentifiers.Id);
            var localUser = await _userManager.FindByIdAsync(userId.Value);
            var savedRefreshToken = localUser.RefreshToken;
            if (savedRefreshToken != refreshToken)
            {
                throw new SecurityTokenException("Invalid refresh token.");
            }

            var newJwtToken = await Tokens.GenerateJwtAsync(_jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id),
                _jwtFactory, localUser.UserName, _jwtIssuerOptions,
                new JsonSerializerSettings {Formatting = Formatting.Indented});

            var newRefreshToken = Tokens.GenerateRefreshToken();
            localUser.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(localUser);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append("sw5e_accessToken", newJwtToken.AuthToken, cookieOptions);
            Response.Cookies.Append("sw5e_refreshToken", newRefreshToken, cookieOptions);

            return Ok(new AuthReplyWithRefreshToken
            {
                UserName = localUser.UserName,
                AccessTokenExpiration = newJwtToken.ExpiresIn
            });
        }

        [HttpGet("logout")]
        public async Task<ActionResult> LogoutAsync()
        {
            var token = Request.Cookies["sw5e_accessToken"];

            var principal = GetPrincipalFromToken(token);
            var userId = principal.FindFirst(c => c.Type == Constants.Strings.JwtClaimIdentifiers.Id);
            var localUser = await _userManager.FindByIdAsync(userId.Value);

            localUser.RefreshToken = null;
            await _userManager.UpdateAsync(localUser);

            Response.Cookies.Delete("sw5e_accessToken");
            Response.Cookies.Delete("sw5e_refreshToken");

            return Ok();
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["TokenSigningKey"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid refresh token.");
            }

            return principal;
        }
    }
}