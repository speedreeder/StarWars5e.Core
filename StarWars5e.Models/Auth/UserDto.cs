using System;
using Microsoft.Azure.Cosmos.Table;

namespace StarWars5e.Models.Auth
{
    public class UserDto : TableEntity
    {
        public string UserName { get; set; }
        public string MostRecentAuthType { get; set; }
        public DateTime? LastLoginTimeUtc { get; set; }
        public DateTime? RegistrationDateUtc { get; set; }

    }
}
