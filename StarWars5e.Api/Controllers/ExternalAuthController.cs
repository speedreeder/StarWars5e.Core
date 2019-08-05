using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StarWars5e.Api.Auth;
using StarWars5e.Api.Helpers;
using StarWars5e.Models.Auth;

namespace StarWars5e.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class ExternalAuthController : ControllerBase
    {
        private readonly FacebookAuthSettings _fbAuthSettings;
        private static readonly HttpClient Client = new HttpClient();
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtFactory _jwtFactory;
        private readonly DiscordAuthSettings _discordAuthSettings;
        private readonly GoogleAuthSettings _googleAuthSettings;
        private readonly JwtIssuerOptions _jwtIssuerOptions;

        public ExternalAuthController(IOptions<FacebookAuthSettings> fbAuthSettings, UserManager<AppUser> userManager,
            JwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtIssuerOptions,
            IOptions<GoogleAuthSettings> googleAuthSettings, IOptions<DiscordAuthSettings> discordAuthSettings)
        {
            _fbAuthSettings = fbAuthSettings.Value;
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _discordAuthSettings = discordAuthSettings.Value;
            _googleAuthSettings = googleAuthSettings.Value;
            _jwtIssuerOptions = jwtIssuerOptions.Value;
        }

        [HttpPost("facebook")]
        public async Task<ActionResult<string>> FacebookAuthAsync([FromBody]FacebookAuthDto model)
        {
            var userAccessTokenResponse = await Client.GetStringAsync(
                $"https://graph.facebook.com/v3.3/oauth/access_token?client_id={_fbAuthSettings.AppId}&redirect_uri={_fbAuthSettings.RedirectUri}&client_secret={_fbAuthSettings.AppSecret}&code={model.Code}");
            var userFacebookAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(userAccessTokenResponse);

            var userInfoResponse = await Client.GetStringAsync(
                $"https://graph.facebook.com/me?fields=email,first_name,last_name,name&access_token={userFacebookAccessToken.AccessToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            var user = await _userManager.FindByEmailAsync(userInfo.Email);

            if (user == null)
            {
                var appUser = new AppUser
                {
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    FacebookId = userInfo.Id,
                    Email = userInfo.Email,
                    UserName = userInfo.Email,
                    MostRecentAuthType = "facebook"
                };

                var result = await _userManager.CreateAsync(appUser);

                if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            }

            var localUser = await _userManager.FindByNameAsync(userInfo.Email);

            if (localUser == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Failed to create local user account.",
                    ModelState));
            }

            var jwt = await Tokens.GenerateJwtAsync(_jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id), _jwtFactory,
                localUser.UserName, _jwtIssuerOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

            var newRefreshToken = Tokens.GenerateRefreshToken();
            localUser.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(localUser);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true
            };
            Response.Cookies.Append("sw5e_accessToken", jwt.AuthToken, cookieOptions);
            Response.Cookies.Append("sw5e_refreshToken", newRefreshToken, cookieOptions);

            return Ok(new AuthReplyWithRefreshToken
            {
                UserName = localUser.UserName,
                AccessTokenExpiration = jwt.ExpiresIn
            });
        }

        [HttpPost("google")]
        public async Task<ActionResult<string>> GoogleAuthAsync([FromBody] FacebookAuthDto model)
        {
            // generate a user access token from access code
            var userAccessTokenResponse = await Client.PostAsync(
                "https://www.googleapis.com/oauth2/v4/token?" +
                $"client_id={_googleAuthSettings.ClientId}" +
                $"&client_secret={_googleAuthSettings.ClientSecret}" +
                "&grant_type=authorization_code" +
                $"&code={model.Code}" +
                $"&redirect_uri={_googleAuthSettings.RedirectUri}", null);

            var userFacebookAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(await userAccessTokenResponse.Content.ReadAsStringAsync());

            // request user data from Google
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", $"{userFacebookAccessToken.AccessToken}");
            var userInfoResponse = await Client.GetStringAsync(
                $"https://www.googleapis.com/userinfo/v2/me");
            var userInfo = JsonConvert.DeserializeObject<GoogleUserData>(userInfoResponse);

            var user = await _userManager.FindByNameAsync(userInfo.Email);

            if (user == null)
            {
                var appUser = new AppUser
                {
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    GoogleId = userInfo.Id,
                    Email = userInfo.Email,
                    UserName = userInfo.Email,
                    MostRecentAuthType = "google"
                };

                var result = await _userManager.CreateAsync(appUser);

                if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            }

            var localUser = await _userManager.FindByNameAsync(userInfo.Email);

            if (localUser == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Failed to create local user account.",
                    ModelState));
            }

            var jwt = await Tokens.GenerateJwtAsync(_jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id), _jwtFactory,
                localUser.UserName, _jwtIssuerOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

            var newRefreshToken = Tokens.GenerateRefreshToken();
            localUser.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(localUser);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true
            };
            Response.Cookies.Append("sw5e_accessToken", jwt.AuthToken, cookieOptions);
            Response.Cookies.Append("sw5e_refreshToken", newRefreshToken, cookieOptions);

            return Ok(new AuthReplyWithRefreshToken
            {
                UserName = localUser.UserName,
                AccessTokenExpiration = jwt.ExpiresIn
            });
        }

        [HttpPost("discord")]
        public async Task<ActionResult<string>> DiscordAuthAsync([FromBody] FacebookAuthDto model)
        {
            // generate a user access token from access code
            var dict = new Dictionary<string, string>
            {
                {"client_id", _discordAuthSettings.ClientId},
                {"client_secret", _discordAuthSettings.ClientSecret},
                {"grant_type", "authorization_code"},
                {"redirect_uri", _discordAuthSettings.RedirectUri},
                {"scope", "identify email"},
                {"code", model.Code}
            };
            var req = new HttpRequestMessage(HttpMethod.Post, "https://discordapp.com/api/oauth2/token") { Content = new FormUrlEncodedContent(dict) };

            var userAccessTokenResponse = await Client.SendAsync(req);

            var userFacebookAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(await userAccessTokenResponse.Content.ReadAsStringAsync());

            // request user data from Google
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", $"{userFacebookAccessToken.AccessToken}");
            var userInfoResponse = await Client.GetStringAsync(
                "https://discordapp.com/api/users/@me");
            var userInfo = JsonConvert.DeserializeObject<DiscordUserData>(userInfoResponse);

            var user = await _userManager.FindByNameAsync(userInfo.Email);

            if (user == null)
            {
                var appUser = new AppUser
                {
                    DiscordId = userInfo.Id,
                    Email = userInfo.Email,
                    UserName = userInfo.Email,
                    MostRecentAuthType = "discord"
                };

                var result = await _userManager.CreateAsync(appUser);

                if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            }

            var localUser = await _userManager.FindByNameAsync(userInfo.Email);

            if (localUser == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Failed to create local user account.",
                    ModelState));
            }

            var jwt = await Tokens.GenerateJwtAsync(_jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id), _jwtFactory,
                localUser.UserName, _jwtIssuerOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });

            var newRefreshToken = Tokens.GenerateRefreshToken();
            localUser.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(localUser);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true
            };
            Response.Cookies.Append("sw5e_accessToken", jwt.AuthToken, cookieOptions);
            Response.Cookies.Append("sw5e_refreshToken", newRefreshToken, cookieOptions);

            return Ok(new AuthReplyWithRefreshToken
            {
                UserName = localUser.UserName,
                AccessTokenExpiration = jwt.ExpiresIn
            });
        }
    }
}
