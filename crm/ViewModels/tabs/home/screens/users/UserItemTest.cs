using crm.Models.appcontext;
using crm.Models.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens.users
{
    public class UserItemTest : UserListItem
    {

        static int cntr;

        public UserItemTest() : base(new ApplicationContext())
        {
            Id = $"{++cntr}";
            Litera = "Y";
            Email = "asknvl@protonmail.com";
            FullName = $"Петров Иван Николаевич";
            LastName = "Петров";
            FirstName = "Иван";
            MiddleName = "Николаевич";
            BirthDate = "28.06.1986";
            PhoneNumber = "+7 (925) 618-69-36";
            SocialNetworks = new List<SocialNetwork> { new SocialNetwork() { Address = "Instagram", Account = "@xeylov" } };
            Telegram = "@xeylov";
            LastEventDate = "04/05/2022 13:23:22";
            LastLoginDate = "04/05/2022 13:23:23";
            Wallet = "0xd003B6F391566Fb2704AAcdA46b502fa2c346898";
            Devices = new List<Device> {
                new Device() { Id = 1, Name = "Mac 1", Serial = "123"},
                new Device() { Id = 2, Name = "Mac 2", Serial = "123"}
            };
            Roles = new List<Role>
            {
                new Role(RoleType.admin)
            };
            SocialNetworks = new List<SocialNetwork>()
            {
                new SocialNetwork()
                {
                    Address = "www.instagram.com/xeylov"
                }
            };

            Status = true;
        }

        public UserItemTest(ApplicationContext appcontext) : base(appcontext)
        {
            Id = $"{++cntr}";
            Litera = "Y";
            Email = "asknvl@protonmail.com";
            FullName = $"Петров Иван Николаевич";
            LastName = "Петров";
            FirstName = "Иван";
            MiddleName = "Николаевич";
            BirthDate = "28.06.1986";
            PhoneNumber = "+7 (925) 618-69-36";
            SocialNetworks = new List<SocialNetwork> { new SocialNetwork() { Address = "Instagram", Account = "@xeylov" } };
            Telegram = "@xeylov";
            LastEventDate = "04/05/2022 13:23:22";
            LastLoginDate = "04/05/2022 13:23:23";
            Wallet = "0xd003B6F391566Fb2704AAcdA46b502fa2c346898";
            Devices = new List<Device> {
                new Device() { Id = 1, Name = "Mac 1", Serial = "123"},
                new Device() { Id = 2, Name = "Mac 2", Serial = "123"}
            };
            Roles = new List<Role>
            {
                new Role(RoleType.admin)
            };
            SocialNetworks = new List<SocialNetwork>()
            {
                new SocialNetwork()
                {
                    Address = "www.instagram.com/xeylov"
                }
            };

            Status = true;
        }
    }
}
