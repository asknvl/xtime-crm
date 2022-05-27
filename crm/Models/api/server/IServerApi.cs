using crm.Models.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.api.server
{
    public interface IServerApi
    {
        Task<bool> ValidateRegToken(string token);
        Task<bool> RegisterUser(string token, BaseUser user);
        Task<BaseUser> Login(string login, string password);
        Task<User> GetUser(string id, string token);
        Task<(List<User>, int, int)> GetUsers(int page, int size, string token);
        Task<string> GetNewUserToken(List<Role> roles, string token);
        Task<bool> UpdateUserInfo(string token, BaseUser user);
    }
}
