using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarWars5e.Models.Auth;

namespace StarWars5e.Api.Controllers
{
    //[Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Microsoft.Azure.Cosmos.Table.CloudTableClient _cosmosTableClient;

        public UserController(Microsoft.Azure.Cosmos.Table.CloudTableClient cosmosTableClient)
        {
            _cosmosTableClient = cosmosTableClient;
        }

        [HttpGet("{userName}")]
        public ActionResult<UserDto> Get(string userName)
        {
            var authAspNetUsers = _cosmosTableClient.GetTableReference("authAspNetUsers");

            var user = authAspNetUsers.CreateQuery<UserDto>().Where(u => u.UserName == userName).ToList().SingleOrDefault();
            return Ok(user);
        }
    }
}
