namespace StarWars5e.Api.Auth
{
    public class AuthReplyWithRefreshToken
    {
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int AccessTokenExpiration { get; set; }
    }
}
