using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StarWars5e.Api.Auth;

namespace StarWars5e.Api.Helpers
{
    public class Tokens
    {
        public static async Task<string> GenerateJwtSerializedAsync(ClaimsIdentity identity,
            JwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions,
            JsonSerializerSettings serializerSettings)
        {
            return JsonConvert.SerializeObject(await GenerateJwtAsync(identity, jwtFactory, userName, jwtOptions, serializerSettings));
        }

        public static async Task<JwtResponse> GenerateJwtAsync(ClaimsIdentity identity, JwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new JwtResponse
            {
                AuthToken = await jwtFactory.GenerateEncodedToken(userName, identity),
                ExpiresIn = (int)jwtOptions.ValidFor.TotalSeconds
            };

            return response;
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}