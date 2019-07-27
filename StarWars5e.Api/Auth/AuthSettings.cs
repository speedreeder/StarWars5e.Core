namespace StarWars5e.Api.Auth
{
    public class AuthSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
    }

    public class DiscordAuthSettings : AuthSettings
    {
    }
}
