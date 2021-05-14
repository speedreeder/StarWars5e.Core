using System.Collections.Generic;
using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Character
{
    public class PostCharacterRequest
    {
        public string Id { get; set; }
        public string JsonData { get; set; }
        public Dictionary<string, CharacterPermissionLevel> UserPermissions { get; set; }
    }
}
