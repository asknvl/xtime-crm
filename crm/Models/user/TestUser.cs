using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.user
{
    class TestUser : User
    {
        public TestUser()
        {

            Email = "test@protonmail.com";
            Password = "F123qwe$%^0000";            
            FullName = "Коновалов Алексей Сергеевич";
            Litera = "XY";
            FirstName = "Алексей";
            MiddleName = "Сергеевич";
            LastName = "Коновалов";
            BirthDate = "28.06.1986";
            PhoneNumber = "+7 (925) 618-69-36";
            Telegram = "@xeylov";
            Wallet = "$$$$$$";

            Devices = new List<Device> {
                new Device() { Id = 1, Name = "Mac 1", Serial = "123"},
                new Device() { Id = 2, Name = "Mac 2", Serial = "123"}
            };

            Roles = new List<Role>
            {
                new Role(RoleType.admin),
                new Role(RoleType.buyer_media)
            };

            SocialNetworks = new List<SocialNetwork>()
            {
                new SocialNetwork()
                {
                    Address = "www.instagram.com/xeylov"
                }
            };

        }
    }
}
