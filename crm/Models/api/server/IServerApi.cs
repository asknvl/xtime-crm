using crm.Models.creatives;
using crm.Models.geoservice;
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
        Task<(List<User>, int, int)> GetUsers(int page, int size, string token, string sortparameter);
        Task<string> GetNewUserToken(List<Role> roles, string token);
        Task<bool> UpdateUserInfo(string token, BaseUser user);
        Task<bool> UpdateUserComment(string token, BaseUser user);
        Task<bool> UpdateUserPassword(string token, BaseUser user, string password);
        Task DeleteUser(string token, BaseUser user);
        Task<List<GEO>> GetGeos(string token, string sortparameter);
        Task<List<CreativeServerDirectory>> GetCreativeServerDirectories(string token);
        Task<(int, string, string)> AddCreative(string token, string filename, string extension, CreativeServerDirectory dir);
        Task SetVisibility(string token, int id, bool isVisible);
        Task SetCreativeStatus(string token, int id, bool isUploaded, bool isVisible);
        Task<(List<CreativeDTO>, int, int)> GetAvaliableCreatives(string token, int page, int size, CreativeServerDirectory dir, int filetype, bool? showinvisible);
    }

    public class CreativeDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string filename { get; set; }
        public int creo_directory_id { get; set; }
        public string geolocation { get; set; }
        public string creo_type { get; set; }
        public string file_extension { get; set; }
        public int file_type_id { get; set; }
        public string file_type { get; set; }
        public bool uploaded { get; set; }
        public bool visibility { get; set; }

    }

}
