namespace StarWars5e.Api.Auth
{
    public class AuthReplyWithRefreshToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int AccessTokenExpiration { get; set; }
    }
}
