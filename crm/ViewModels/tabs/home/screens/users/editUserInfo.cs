using Avalonia.Controls.Selection;
using Avalonia.Data;
using crm.Models.appcontext;
using crm.Models.autocompletions;
using crm.Models.user;
using crm.Models.validators;
using crm.ViewModels.dialogs;
using crm.ViewModels.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens.users
{
    public class editUserInfo : BaseScreen, ICommandActions
    {
        #region vars
        IValidator<string> fn_vl = new FullNameValidator();
        IValidator<string> litera_vl = new LiteraValidator();
        IAutoComplete email_ac = new EmailAutoComplete();
        IValidator<string> email_vl = new LoginValidator();
        IValidator<string> phone_vl = new PhoneNumberValidator();
        IValidator<string> birth_vl = new BirthDateValidator();
        IValidator<string> tg_vl = new TelegramValidator();
        IValidator<string> wallet_vl = new WalletValidator();

        bool
           isEmail,
           isFullName,
           isLitera,
           isBirthDate,
           isPhoneNumber,
           isTelegram,
           isWallet,
           isRoles;

        TagsAndRolesConvetrer convetrer = new();

        BaseUser user;
        string token;
        #endregion

        #region properties
        public override string Title => "Редактировать";        

        bool isEditable;
        public bool IsEditable { 
            get => isEditable;
            set => this.RaiseAndSetIfChanged(ref isEditable, value);
        }

        bool isInputValid = true;
        public bool IsInputValid
        {
            get => isInputValid;
            set
            {
                this.RaiseAndSetIfChanged(ref isInputValid, value);
            }
        }

        string fullname;
        public string FullName
        {
            get => fullname;
            set
            {
                isFullName = fn_vl.IsValid(value);
                //updateValidity();
                //if (!isFullName) { 
                //    throw new DataValidationException(fn_vl.Message);                
                if (!isFullName)
                    AddError(nameof(FullName), fn_vl.Message);
                else
                    RemoveError(nameof(FullName));
                updateValidity();
                this.RaiseAndSetIfChanged(ref fullname, value);
            }
        }

        string litera;
        public string Litera
        {
            get => litera;
            set
            {
                isLitera = litera_vl.IsValid(value);
                //updateValidity();
                //if (!isFullName) { 
                //    throw new DataValidationException(fn_vl.Message);                
                if (!isFullName)
                    AddError(nameof(Litera), litera_vl.Message);
                else
                    RemoveError(nameof(Litera));
                updateValidity();
                this.RaiseAndSetIfChanged(ref litera, value);
            }
        }

        string email;
        public string Email
        {
            get => email;
            set
            {
                isEmail = email_vl.IsValid(value);
                //updateValidity();
                //if (!isEmail)
                //    throw new DataValidationException("Введен некорректный e-mail");                
                if (!isEmail)
                    AddError(nameof(Email), fn_vl.Message);
                else
                    RemoveError(nameof (Email));
                updateValidity();
                this.RaiseAndSetIfChanged(ref email, value);
            }
        }

        string phonenumber;
        public string PhoneNumber
        {
            get => phonenumber;
            set
            {
                isPhoneNumber = phone_vl.IsValid(value);
                //updateValidity();
                //if (!isPhoneNumber)
                //    throw new DataValidationException(phone_vl.Message);                
                if (!isPhoneNumber)
                    AddError(nameof(PhoneNumber), phone_vl.Message);
                else
                    RemoveError(nameof(PhoneNumber));
                updateValidity();
                this.RaiseAndSetIfChanged(ref phonenumber, value);
            }
        }

        string birthdate;
        public string BirthDate
        {
            get => birthdate;
            set
            {
                isBirthDate = birth_vl.IsValid(value);
                //updateValidity();
                //if (!isBirthDate)
                //    throw new DataValidationException(birth_vl.Message);                
                if (!isBirthDate)
                    AddError(nameof(BirthDate), birth_vl.Message);
                else
                    RemoveError(nameof(BirthDate));
                updateValidity();
                this.RaiseAndSetIfChanged(ref birthdate, value);
            }
        }

        public ObservableCollection<SocialNetwork> SocialNetworks { get; } = new ObservableCollection<SocialNetwork>();

        string telegram;
        public string Telegram
        {
            get => telegram;
            set
            {
                isTelegram = tg_vl.IsValid(value);
                //updateValidity();
                //if (!isTelegram)
                //    throw new DataValidationException(tg_vl.Message);                
                if (!isTelegram)
                    AddError(nameof(Telegram), tg_vl.Message);
                else
                    RemoveError(nameof(Telegram));                    
                this.RaiseAndSetIfChanged(ref telegram, value);
            }
        }

        string wallet;
        public string Wallet
        {
            get => wallet;
            set
            {
                isWallet = wallet_vl.IsValid(value);
                //updateValidity();
                //if (!isWallet)
                //    throw new DataValidationException(wallet_vl.Message);                
                if (!isWallet)
                    AddError(nameof(Wallet), wallet_vl.Message);
                else
                    RemoveError(nameof(Wallet));
                updateValidity();
                this.RaiseAndSetIfChanged(ref wallet, value);
            }
        }

        public List<tagsListItem> Tags { get; set; } = new();
        public List<tagsListItem> SelectedTags { get; set; } = new();
        public SelectionModel<tagsListItem> Selection { get; set; }

        #endregion

        #region commands        
        public ReactiveCommand<Unit, Unit> openTelegram { get; }        
        #endregion

        public editUserInfo() : base(new ApplicationContext())
        {        

            TestUser user = new TestUser();
            FullName = user.FullName;
            Litera = user.Litera;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            BirthDate = user.BirthDate;
            Telegram = user.Telegram;
            Wallet = user.Wallet;

            foreach (var item in user.SocialNetworks)
                SocialNetworks.Add(item);

            //SelectedTags = convetrer.RolesToTags(user.Roles);

            Tags = convetrer.GetAllTagsList();
            Selection = new SelectionModel<tagsListItem>();
            Selection.SingleSelect = false;           
            SelectedTags = convetrer.RolesToTags(user.Roles);
            foreach (var item in SelectedTags) {
                int index = Tags.IndexOf(Tags.FirstOrDefault(t => t.Name.Equals(item.Name)));
                Selection.Select(index);
            }

            Selection.SelectionChanged += Selection_SelectionChanged;

            openTelegram = ReactiveCommand.Create(() => {
                Process.Start(new ProcessStartInfo
                {
                    FileName = $"tg://resolve?domain={Telegram}",
                    UseShellExecute = true
                });
            });         
            
        }

        public editUserInfo(ApplicationContext context, BaseUser user) : base(context)
        {
            this.user = user;
            token = AppContext.User.Token;

            Tags = convetrer.GetAllTagsList();
            Selection = new SelectionModel<tagsListItem>();
            Selection.SingleSelect = false;

            init(user);

            openTelegram = ReactiveCommand.Create(() => {
                Process.Start(new ProcessStartInfo
                {
                    FileName = $"tg://resolve?domain={Telegram}",
                    UseShellExecute = true
                });
            });
        }

        #region helpers
        void init(BaseUser user)
        {
            
            FullName = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            BirthDate = user.BirthDate;
            Telegram = user.Telegram;
            Wallet = user.Wallet;

            foreach (var item in user.SocialNetworks)
                SocialNetworks.Add(item);

            Selection.Clear();
            Selection.SelectionChanged -= Selection_SelectionChanged;
            SelectedTags = convetrer.RolesToTags(user.Roles);
            foreach (var item in SelectedTags)
            {
                int index = Tags.IndexOf(Tags.FirstOrDefault(t => t.Name.Equals(item.Name)));
                Selection.Select(index);
            }
            Selection.SelectionChanged += Selection_SelectionChanged;

        }
        protected bool CheckValidity(bool[] fields)
        {
            return !fields.Any(p => p == false);
        }
        void updateValidity()
        {
            IsInputValid = CheckValidity(new bool[] {
                isEmail,
                isFullName,
                isBirthDate,
                isPhoneNumber,
                isTelegram,
                isWallet,
                isRoles
            });
        }
        #endregion

        #region private
        private void Selection_SelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<tagsListItem> e)
        {

            foreach (var item in e.SelectedItems)
            {
                SelectedTags.Add(item);
            }

            foreach (var item in e.DeselectedItems)
            {
                SelectedTags.Remove(item);
            }

            bool isTeamLead = SelectedTags.Any(t => t.Name.Equals(Role.teamlead));
            bool isAdmin = SelectedTags.Any(t => t.Name.Equals(Role.admin));
            bool isAnyOne = SelectedTags.Any(t => !t.Name.Equals(Role.teamlead) && !t.Name.Equals(Role.admin));

            isRoles = (isAdmin && !isTeamLead) || (isTeamLead && isAnyOne) || isAnyOne;
            updateValidity();

        }
        #endregion

        #region public
        public async Task<bool> Save()
        {

            BaseUser updUser = new User();
            updUser.Copy(user);

            updUser.Email = Email;
            updUser.FullName = FullName;
            updUser.BirthDate = BirthDate;
            updUser.PhoneNumber = PhoneNumber;
            updUser.Telegram = Telegram;
            updUser.Wallet = Wallet;
            updUser.Roles = convetrer.TagsToRoles(SelectedTags);

            return await AppContext.ServerApi.UpdateUserInfo(token, updUser);
        }

        public void Cancel()
        {
            init(user);
        }

        #endregion

    }
}
