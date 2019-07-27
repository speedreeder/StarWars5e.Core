namespace StarWars5e.Api.Auth
{
    public class JwtResponse
    {
        public string AuthToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
