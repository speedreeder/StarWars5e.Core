using System.Collections.Generic;
using StarWars5e.Models.User;
using System.Threading.Tasks;

namespace StarWars5e.Api.Interfaces
{
    public interface IUserManager
    {
        Task<List<User>> SearchUsersAsync(string username = "", string id = "");
    }
}
