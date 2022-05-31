using Avalonia;
using Avalonia.Data;
using crm.Models.autocompletions;
using crm.Models.validators;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TextCopy;
using crm.Models.user;
using crm.Models.api.server;
using crm.WS;
using crm.ViewModels.dialogs;
using System.ComponentModel;
using System.Collections;
using crm.Models.appcontext;
using crm.ViewModels.tabs.tabservice;

namespace crm.ViewModels.tabs
{
    public class registrationVM : Tab
    {
        #region vars
        bool
            isEmail,
            isPassword1,
            isPassword2,
            isPasswordsEql,
            isFullName,
            isBirthDate,
            isPhoneNumber,
            isTelegram,
            isWallet,
            isDevice;

        IValidator<string> email_vl = new LoginValidator();
        IAutoComplete email_ac = new EmailAutoComplete();
        IValidator<string> pswrd_vl = new PasswordValidator_server();
        IValidator<string> fn_vl = new FullNameValidator();
        IValidator<string> birth_vl = new BirthDateValidator();
        IAutoComplete birth_ac = new BirtDateAutoCompletion();
        IValidator<string> phone_vl = new PhoneNumberValidator();
        IValidator<string> tg_vl = new TelegramValidator();
        IValidator<string> wallet_vl = new WalletValidator();

        IWindowService ws = WindowService.getInstance();

        string token;
        #endregion

        #region properties
        string email;
        public string Email
        {
            get => email;
            set
            {
                value = email_ac.AutoComplete(value);
                isEmail = email_vl.IsValid(value);
                updateValidity();
                if (!isEmail)
                    throw new DataValidationException("Введен некорректный e-mail");
                this.RaiseAndSetIfChanged(ref email, value);
            }
        }

        string password1;
        public string Password1
        {
            get => password1;
            set
            {
                isPassword1 = pswrd_vl.IsValid(value);
                if (isPassword1)
                {
                    RemoveError(nameof(Password1));

                    if (!string.IsNullOrEmpty(Password2))
                    {
                        isPasswordsEql = value == Password2;
                        if (!isPasswordsEql)
                        {
                            AddError(nameof(Password1), "Пароли должны совпадать");
                            //AddError(nameof(Password2), "Пароли должны совпадать");
                        } else
                        {
                            RemoveError(nameof(Password1));
                            RemoveError(nameof(Password2));
                        }
                    }

                } else
                    AddError(nameof(Password1), pswrd_vl.Message);

                updateValidity();
                this.RaiseAndSetIfChanged(ref password1, value);
            }
        }

        string password2;
        public string Password2
        {
            get => password2;
            set
            {
                isPassword2 = pswrd_vl.IsValid(value);
                if (isPassword2)
                {
                    RemoveError(nameof(Password2));

                    if (!string.IsNullOrEmpty(Password1))
                    {
                        isPasswordsEql = value == Password1;
                        if (!isPasswordsEql)
                        {
                            //AddError(nameof(Password1), "Пароли должны совпадать");
                            AddError(nameof(Password2), "Пароли должны совпадать");
                        } else
                        {
                            RemoveError(nameof(Password1));
                            RemoveError(nameof(Password2));
                        }
                    }
                } else
                    AddError(nameof(Password2), pswrd_vl.Message);

                updateValidity();
                this.RaiseAndSetIfChanged(ref password2, value);
            }
        }

        string fullname;
        public string FullName
        {
            get => fullname;
            set
            {
                isFullName = fn_vl.IsValid(value);
                updateValidity();
                if (!isFullName)
                    throw new DataValidationException(fn_vl.Message);
                this.RaiseAndSetIfChanged(ref fullname, value);
            }
        }

        string birthdate;
        public string BirthDate
        {
            get => birthdate;
            set
            {
                value = birth_ac.AutoComplete(value);
                isBirthDate = birth_vl.IsValid(value);
                updateValidity();
                if (!isBirthDate)
                    throw new DataValidationException(birth_vl.Message);
                this.RaiseAndSetIfChanged(ref birthdate, value);
            }
        }

        string phonenumber;
        public string PhoneNumber
        {
            get => phonenumber;
            set
            {
                isPhoneNumber = phone_vl.IsValid(value);
                updateValidity();
                if (!isPhoneNumber)
                    throw new DataValidationException(phone_vl.Message);
                this.RaiseAndSetIfChanged(ref phonenumber, value);
            }
        }

        string telegram;
        public string Telegram
        {
            get => telegram;
            set
            {
                isTelegram = tg_vl.IsValid(value);
                updateValidity();
                if (!isTelegram)
                    throw new DataValidationException(tg_vl.Message);
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
                updateValidity();
                if (!isWallet)
                    throw new DataValidationException(wallet_vl.Message);
                this.RaiseAndSetIfChanged(ref wallet, value);
            }
        }

        public string Token { get; set; }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> pasteCmd { get; }
        public ReactiveCommand<Unit, Unit> registerCmd { get; }
        #endregion

        public registrationVM(ITabService ts, ApplicationContext appcontext) : base(ts)
        {
            Title = "Регистрация";
#if DEBUG
            Email = "test@protonmail.com";
            //Password1 = "F123qwe$%^0000";
            //Password2 = "F123qwe$%^0000";
            FullName = "Коновалов Алексей Сергеевич";
            BirthDate = "28.06.1986";
            PhoneNumber = "+7 (925) 618-69-36";
            Telegram = "@xeylov";
            //Wallet = "$$$$$$";            
#endif

            #region commands
            pasteCmd = ReactiveCommand.CreateFromTask(async () =>
            {
                Clipboard clipboard = new Clipboard();
                try
                {
                    Wallet = clipboard.GetText();
                } catch { }
            });

            registerCmd = ReactiveCommand.CreateFromTask(async () =>
            {

                User user = new User()
                {
                    Email = Email,
                    Password = Password1,
                    FullName = FullName,
                    BirthDate = BirthDate,
                    PhoneNumber = PhoneNumber,
                    Telegram = Telegram,
                    Wallet = Wallet,
                };

                string[] splt = FullName.Split(" ");
                user.FirstName = splt[1];
                user.MiddleName = splt[2];
                user.LastName = splt[0];

                bool res = false;
                try
                {
                    res = await appcontext.ServerApi.RegisterUser(Token, user);
                    if (res)
                        onUserRegistered?.Invoke();

                } catch (Exception ex)
                {
                    ws.ShowDialog(new errMsgVM(ex.Message));
                }

            });
            #endregion

        }

        #region helpers
        void updateValidity()
        {
            IsInputValid = CheckValidity(new bool[] {
                isEmail,
                isPassword1,
                isPassword2,
                isPasswordsEql,
                isFullName,
                isBirthDate,
                isPhoneNumber,
                isTelegram,
                isWallet                
                //isDevice
                //true
            });
        }
        #endregion

        #region public
        public event Action onUserRegistered;
        public override void Clear()
        {
        }
        #endregion
    }
}
