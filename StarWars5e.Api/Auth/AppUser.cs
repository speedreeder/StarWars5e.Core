using ElCamino.AspNetCore.Identity.AzureTable.Model;

namespace StarWars5e.Api.Auth
{
    public class AppUser : IdentityUserV2
    {
        public long FacebookId { get; set; }
        public string GoogleId { get; set; }
        public string DiscordId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RefreshToken { get; set; }
        public string MostRecentAuthType { get; set; }
    }
}
