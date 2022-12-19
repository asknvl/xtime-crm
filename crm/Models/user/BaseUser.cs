using crm.ViewModels;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.user
{
    public abstract class BaseUser : ViewModelBase
    {
        #region properties
        string? id;
        [JsonProperty("id")]
        public virtual string? Id
        {
            get => id;
            set => this.RaiseAndSetIfChanged(ref id, value);
        }
        string? token;
        public string? Token { get; set; }

        string? litera;
        [JsonProperty("letter_id")]
        public string? Litera
        {
            get => litera;
            set => this.RaiseAndSetIfChanged(ref litera, value);
        }

        string? email;
        [JsonProperty("email")]
        public string? Email
        {
            get => email;
            set => this.RaiseAndSetIfChanged(ref email, value);
        }
        string? password;
        public string? Password
        {
            get => password;
            set => this.RaiseAndSetIfChanged(ref password, value);
        }
        string? fullname;
        [JsonProperty("fullname")]
        public string? FullName
        {
            get => fullname;
            set => this.RaiseAndSetIfChanged(ref fullname, value);
        }

        string? lastname;
        [JsonProperty("lastname")]
        public string? LastName
        {
            get => lastname;
            set => this.RaiseAndSetIfChanged(ref lastname, value);
        }

        string? firstname;
        [JsonProperty("firstname")]
        public string? FirstName
        {
            get => firstname;
            set => this.RaiseAndSetIfChanged(ref firstname, value);
        }

        string? middlename;
        [JsonProperty("middlename")]
        public string? MiddleName
        {
            get => middlename;
            set => this.RaiseAndSetIfChanged(ref middlename, value);
        }

        string? birthdate;
        [JsonProperty("birthday")]
        public string? BirthDate
        {
            get => birthdate;
            set => this.RaiseAndSetIfChanged(ref birthdate, value);
        }
        string? phonenumber;
        [JsonProperty("phone")]
        public string? PhoneNumber
        {
            get => phonenumber;
            set => this.RaiseAndSetIfChanged(ref phonenumber, value);
        }

        List<SocialNetwork> socialnetworks;
        [JsonProperty("social_networks")]
        public List<SocialNetwork> SocialNetworks
        {
            get => socialnetworks;
            set => this.RaiseAndSetIfChanged(ref socialnetworks, value);
        }

        string? telegram;
        [JsonProperty("telegram")]
        public string? Telegram
        {
            get => telegram;
            set => this.RaiseAndSetIfChanged(ref telegram, value);
        }
        string? wallet;
        [JsonProperty("usdt_account")]
        public string? Wallet
        {
            get => wallet;
            set => this.RaiseAndSetIfChanged(ref wallet, value);
        }

        bool? isconnected;
        [JsonProperty("is_connected")]
        public bool ? IsConnected
        {
            get => isconnected;
            set => this.RaiseAndSetIfChanged(ref isconnected, value);
        }

        string? lastlogindate;
        [JsonProperty("last_login_date")]
        public string? LastLoginDate
        {
            get => lastlogindate;
            set => this.RaiseAndSetIfChanged(ref lastlogindate, value);
        }

        string? lasteventdate;
        [JsonProperty("last_event_date")]
        public string? LastEventDate
        {
            get => lasteventdate;
            set => this.RaiseAndSetIfChanged(ref lasteventdate, value);
        }

        List<Device>? devices;
        [JsonProperty("devices")]
        public List<Device>? Devices
        {
            get => devices;
            set => this.RaiseAndSetIfChanged(ref devices, value);
        }

        List<Role>? roles;
        [JsonProperty("roles")]
        public List<Role>? Roles
        {
            get => roles;
            set => this.RaiseAndSetIfChanged(ref roles, value);
        }

        string description;
        [JsonProperty("description")]
        public string Description
        {
            get => description;
            set => this.RaiseAndSetIfChanged(ref description, value);
        }

        bool enabled;
        [JsonProperty("enabled")]
        public bool Enabled
        {
            get => enabled;
            set => this.RaiseAndSetIfChanged(ref enabled, value);
        }

        string hire_date;
        [JsonProperty("hire_date")]
        public string HireDate
        {
            get => hire_date;
            set => this.RaiseAndSetIfChanged(ref hire_date, value);
        }

        string dismissal_date;
        [JsonProperty("dismissal_date")]
        public string DismissalDate
        {
            get => dismissal_date;
            set => this.RaiseAndSetIfChanged(ref dismissal_date, value);
        }
        #endregion

        #region public
        public void Copy(BaseUser user)
        {
            Id = user.Id;
            Token = user.Token;

            string[] splt = user.Litera.Split(".");
            if (splt.Length > 1)
            {
                Litera = splt[1];
            } else 
                Litera = user.Litera;

            Email = user.Email;
            Password = user.Password;
            FullName = user.FullName;
            LastName = user.LastName;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            BirthDate = user.BirthDate;
            PhoneNumber = user.PhoneNumber;
            LastLoginDate = user.LastLoginDate;
            LastEventDate = user.LastEventDate;
            IsConnected = user.IsConnected;
            Description = user.Description;

            SocialNetworks = new List<SocialNetwork>();
            foreach (var item in user.SocialNetworks)
                SocialNetworks.Add(new SocialNetwork() { Id = item.Id, Address = item.Address, Account = item.Account });

            Telegram = user.Telegram;
            Wallet = user.Wallet;
            LastLoginDate = user.LastLoginDate;
            LastEventDate = user.LastEventDate;

            Devices = new List<Device>();
            foreach (var item in user.Devices)
                Devices.Add(new Device() { Id = item.Id, Name = item.Name, Serial = item.Serial });

            Roles = new List<Role>();
            foreach (var item in user.Roles)
                Roles.Add(new Role(item.Id) { Id = item.Id, Name = item.Name });

            HireDate = user.HireDate;
            DismissalDate = user.DismissalDate;

        }
        #endregion

        #region callbacks
        public event Action LoginEvent;
        public event Action LogoutEvent;
        #endregion

    }
}
