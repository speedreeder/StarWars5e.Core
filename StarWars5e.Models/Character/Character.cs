using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Character
{
    public class Character : BaseEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        [IgnoreProperty]
        public string JsonData { get; set; }

        [IgnoreProperty]
        public Dictionary<string, CharacterPermissionLevel> UserPermissions { get; set; }
        public string UserPermissionsJson
        {
            get => UserPermissions == null ? "" : JsonConvert.SerializeObject(UserPermissions);
            set => UserPermissions = JsonConvert.DeserializeObject<Dictionary<string, CharacterPermissionLevel>>(value);
        }
    }
}
