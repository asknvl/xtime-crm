using Avalonia.Controls.Selection;
using Avalonia.Data;
using crm.Models.appcontext;
using crm.Models.autocompletions;
using crm.Models.user;
using crm.Models.validators;
using crm.ViewModels.dialogs;
using crm.ViewModels.Helpers;
using crm.WS;
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
        #region const
        const string no_change_password = "******";
        #endregion

        #region vars
        IValidator<string> fn_vl = new FullNameValidator();
        IValidator<string> litera_vl = new LiteraValidator();
        IAutoComplete email_ac = new EmailAutoComplete();
        IValidator<string> email_vl = new LoginValidator();
        IValidator<string> phone_vl = new PhoneNumberValidator();
        IValidator<string> birth_vl = new BirthDateValidator();
        IValidator<string> tg_vl = new TelegramValidator();
        IValidator<string> wallet_vl = new WalletValidator();
        IValidator<string> pswrd_vl = new PasswordValidator_server();

        bool
           isEmail,
           isFullName,
           isLitera,
           isBirthDate,
           isPhoneNumber,
           isTelegram,
           isWallet,
           isRoles,
           isPassword;

        TagsAndRolesConvetrer convetrer = new();
        BaseUser User;
        string token;

        //commentDlgVM userComment;

        IWindowService ws = WindowService.getInstance();
        #endregion

        #region properties
        public override string Title => "Обзор";        

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
                if (!isLitera)
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
        public ObservableCollection<SocialNetwork> SocialNetworks { get; set; } = new ObservableCollection<SocialNetwork>();

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
                
        public string Description { get; set; }

        string password;
        public string Password
        {
            get => password;
            set
            {
                //isPassword = pswrd_vl.IsValid(value);
                //if (isPassword)
                //{
                //    RemoveError(nameof(Password));                   
                //} else
                //    AddError(nameof(Password), pswrd_vl.Message);

                //updateValidity();
                this.RaiseAndSetIfChanged(ref password, value);
            }
        }

        public List<tagsListItem> Tags { get; set; } = new();
        public List<tagsListItem> SelectedTags { get; set; } = new();
        public SelectionModel<tagsListItem> Selection { get; set; }

        #endregion

        #region commands        
        public ReactiveCommand<Unit, Unit> openTelegramCmd { get; }
        public ReactiveCommand<Unit, Unit> showCommentsCmd { get; }
        #endregion

        public editUserInfo() : base()
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


            Tags = convetrer.GetAllTagsList();
            Selection = new SelectionModel<tagsListItem>();
            Selection.SingleSelect = false;           
            SelectedTags = convetrer.RolesToTags(user.Roles);
            foreach (var item in SelectedTags) {
                int index = Tags.IndexOf(Tags.FirstOrDefault(t => t.Name.Equals(item.Name)));
                Selection.Select(index);
            }

            Selection.SelectionChanged += Selection_SelectionChanged;

            openTelegramCmd = ReactiveCommand.Create(() => {
                Process.Start(new ProcessStartInfo
                {
                    FileName = $"tg://resolve?domain={Telegram}",
                    UseShellExecute = true
                });
            });

            showCommentsCmd = ReactiveCommand.Create(() => {
                
            });
        }

        public editUserInfo(BaseUser user) : base()
        {
            User = user;
            token = AppContext.User.Token;

            Tags = convetrer.GetAllTagsList();
            Selection = new SelectionModel<tagsListItem>();
            Selection.SingleSelect = false;

            init(User);

            openTelegramCmd = ReactiveCommand.Create(() => {
                Process.Start(new ProcessStartInfo
                {
                    FileName = $"tg://resolve?domain={Telegram}",
                    UseShellExecute = true
                });
            });
            
            showCommentsCmd = ReactiveCommand.CreateFromTask(async () => {

                BaseUser user = null;

                try
                {
                    user = await AppContext.ServerApi.GetUser(User.Id, AppContext.User.Token);

                } catch (Exception ex)
                {
                    ws.ShowDialog(new errMsgVM(ex.Message));
                }

                commentDlgVM userComment = new commentDlgVM(user.Description, true);

                userComment.ClosingEvent += (s) =>
                {
                    user.Description = s;

                    try
                    {
                        AppContext.ServerApi.UpdateUserComment(AppContext.User.Token, user);

                    } catch (Exception ex)
                    {
                        ws.ShowDialog(new errMsgVM(ex.Message));
                    }
                };

                ws.ShowDialog(userComment);

            });
        }

        #region helpers
        void init(BaseUser user)
        {
            
            FullName = user.FullName;
            Litera = user.Litera;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            BirthDate = user.BirthDate;
            Telegram = user.Telegram;
            Wallet = user.Wallet;
            Description = user.Description;
            Password = no_change_password;

            //foreach (var item in user.SocialNetworks)
            //    SocialNetworks.Add(item);
            //if (SocialNetworks.Count == 0)
            //    SocialNetworks.Add(new SocialNetwork());

            SocialNetworks.Clear();
            
            if (user.SocialNetworks.Count > 0)
                SocialNetworks.Add(new SocialNetwork() { Account = user.SocialNetworks[0].Account });
            else
                SocialNetworks.Add(new SocialNetwork());


            //SocialNetworks[0].Account = user.SocialNetworks[0].Account;

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
                isRoles,
                isPassword
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
            updUser.Copy(User);

            updUser.Email = Email;
            updUser.FullName = FullName;
            updUser.Litera = Litera;
            updUser.BirthDate = BirthDate;
            updUser.PhoneNumber = PhoneNumber;
            updUser.Telegram = Telegram;
            updUser.Wallet = Wallet;
            updUser.Roles = convetrer.TagsToRoles(SelectedTags);
            updUser.Description = Description;

            updUser.SocialNetworks = new List<SocialNetwork>();
            updUser.SocialNetworks.Add(new SocialNetwork() { Account = SocialNetworks[0].Account });

            bool usr = await AppContext.ServerApi.UpdateUserInfo(token, updUser);            
            bool psw = true;

            if (!Password.Equals(no_change_password))
            {
                psw = await AppContext.ServerApi.UpdateUserPassword(token, updUser, Password);
            }

            return usr & psw;
        }

        public void Cancel()
        {
            init(User);
        }

        #endregion

    }
}
