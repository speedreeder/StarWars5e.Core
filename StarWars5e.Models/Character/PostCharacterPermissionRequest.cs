using StarWars5e.Models.Enums;

namespace StarWars5e.Models.Character
{
    public class PostCharacterPermissionRequest
    {
        public string Id { get; set; }
        public CharacterPermissionLevel RequestedPermissionLevel { get; set; }
        public string UserName { get; set; }
    }
}
