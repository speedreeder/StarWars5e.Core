using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models.Enums;
using StarWars5e.Models.User;

namespace StarWars5e.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAzureTableStorage _tableStorage;
        private readonly IUserManager _userManager;

        public UserController(IAzureTableStorage tableStorage, IUserManager userManager)
        {
            _tableStorage = tableStorage;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(PostUserRequest userRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("No userId found.");
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("No userId found.");
            }

            if (string.IsNullOrWhiteSpace(userRequest.Username))
            {
                return BadRequest("Username cannot be empty.");
            }

            if (userRequest.Username.Length > 30)
            {
                return BadRequest("Username cannot be longer than 30 characters.");
            }

            var users = await _userManager.SearchUsersAsync(userRequest.Username);

            if (users.Any(u => u.Id != userId))
            {
                return BadRequest("Username is taken.");
            }

            var user = new User
            {
                ContentSourceEnum = ContentSource.Community,
                ContentTypeEnum = ContentType.CommunityContent,
                RowKey = userId,
                PartitionKey = ContentType.CommunityContent.ToString(),
                Id = userId,
                Username = userRequest.Username
            };

            await _tableStorage.AddOrUpdateAsync("users", user);

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetUserForCurrentIdentity()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("No userId found.");
            }

            var users = await _userManager.SearchUsersAsync("", userId);

            return Ok(users.SingleOrDefault() ?? new User
            {
                Id = userId
            });
        }

        [HttpGet("search")]
        public async Task<ActionResult<User>> SearchUsers(SearchUserRequest searchUserRequest)
        {
            var users = await _userManager.SearchUsersAsync(searchUserRequest.Username, searchUserRequest.Id);
            return Ok(users);
        }
    }
}
