using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using StarWars5e.Api.Interfaces;
using StarWars5e.Api.Storage;
using StarWars5e.Models.User;

namespace StarWars5e.Api.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IAzureTableStorage _tableStorage;

        public UserManager(IAzureTableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<List<User>> SearchUsersAsync(string username = "", string id = "")
        {
            var filter = "";
            if (!string.IsNullOrEmpty(username))
            {
                filter = $"Username eq '{username}'";
            }
            if (!string.IsNullOrEmpty(id))
            {
                if (!string.IsNullOrEmpty(filter)) filter = $"{filter} and";
                filter = $"{filter} Id eq '{id}'";
            }

            var query = new TableQuery<User>().Where(filter);
            var users = await _tableStorage.QueryAsync("users", query);

            return users.ToList();
        }
    }
}
