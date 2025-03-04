using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GalleryMobile.DataPersistence.Entities;
using GalleryMobile.DataPersistence.Services;
using GalleryMobile.MVVM.View.Pages;
using System.Text.RegularExpressions;

namespace GalleryMobile.MVVM.ViewModel
{
    public partial class RegistrationViewModel : ObservableObject
    {
        private string email = string.Empty;
        private string password = string.Empty;
        private string nickname = string.Empty;
        private string notifyLabel = string.Empty;
        private string passwordEntryColor = "WhiteSmoke";
        private string nicknameEntryColor = "WhiteSmoke";
        private string emailEntryColor = "WhiteSmoke";
        private readonly IGalleryAppDatabaseService database;
        private readonly CancellationTokenSource cancellationTokenSource;

        public RegistrationViewModel(IGalleryAppDatabaseService database,
                                     CancellationTokenSource cancellationTokenSource)
        {
            NotifyLabel = string.Empty;
            this.database = database;
            this.cancellationTokenSource = cancellationTokenSource;
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (email == value)
                {
                    return;
                }
                email = value;
                OnPropertyChanged();
                EmailChanged();
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (password == value)
                {
                    return;
                }
                password = value;
                OnPropertyChanged();
                PasswordChanged();
            }
        }
        public string NickName
        {
            get
            {
                return nickname;
            }
            set
            {
                if (nickname == value)
                {
                    return;
                }
                nickname = value;
                OnPropertyChanged();
            }
        }
        public string NotifyLabel
        {
            get
            {
                return notifyLabel;
            }
            set
            {
                if (notifyLabel == value)
                {
                    return;
                }
                notifyLabel = value;
                OnPropertyChanged();
            }
        }
        public string PasswordEntryColor
        {
            get
            {
                return passwordEntryColor;
            }
            set
            {
                if (passwordEntryColor == value)
                {
                    return;
                }
                passwordEntryColor = value;
                OnPropertyChanged();
            }
        }
        public string NicknameEntryColor
        {
            get
            {
                return nicknameEntryColor;
            }
            set
            {
                if (nicknameEntryColor == value)
                {
                    return;
                }
                nicknameEntryColor = value;
                OnPropertyChanged();
            }
        }
        public string EmailEntryColor
        {
            get
            {
                return emailEntryColor;
            }
            set
            {
                if (emailEntryColor == value)
                {
                    return;
                }
                emailEntryColor = value;
                OnPropertyChanged();
            }
        }

        [RelayCommand]
        public async Task RegisterAsync()
        {
            await Register();
        }
        [RelayCommand]
        public async Task BackAsync()
        {
            await GoBack();
        }
        [RelayCommand]
        public void PasswordChanged()
        {
            PasswordEntryChanged();
        }
        [RelayCommand]
        public void EmailChanged()
        {
            EmailEntryChanged();
        }
        public async Task Register()
        {
            if (Email == string.Empty)
            {
                NotifyLabel = "Email is empty!";
                EmailEntryColor = "Red";
                return;
            }
            if (Password == string.Empty)
            {
                NotifyLabel = "Password is empty!";
                PasswordEntryColor = "Red";
                return;
            }
            if (NickName == string.Empty)
            {
                NotifyLabel = "NickName is empty!";
                NicknameEntryColor = "Red";
                return;
            }
            if (await database.GetUserByEmailAsync(Email, cancellationTokenSource.Token) != null)
            {
                NotifyLabel = "Email is not available!";
                EmailEntryColor = "Red";
                return;
            }
            if (await database.GetUserByNickNameAsync(NickName, cancellationTokenSource.Token) != null)
            {
                NotifyLabel = "Nickname is not available!";
                NicknameEntryColor = "Red";
                return;
            }
            if (!IsValidPassword(Password))
            {
                PasswordEntryColor = "Red";
                NotifyLabel = "Invalid password format! Password must contain: \n·At least 8 characters\n·One uppercase letter\n·One lowercase letter\n·One digit\n·One special character";
                return;
            }
            if (!IsValidEmail(Email))
            {
                EmailEntryColor = "Red";
                NotifyLabel = "Invalid email format!";
                return;
            }
            if (!IsValidNickname(NickName))
            {
                PasswordEntryColor = "Red";
                NotifyLabel = "Invalid nickname format! Nickname must contain: \n·At least 3 characters\n·Only letters,digits and _";
                return;
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

            var user = new User(NickName, hashedPassword, Email, true, DateTime.Now);
            await database.SaveUserAsync(user, cancellationTokenSource.Token);

            NickName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;

            var navigationParameters = new Dictionary<string, object>
            {
                {"CurrentUser", user }
            };

            await Shell.Current.GoToAsync(nameof(MainPage), navigationParameters);
        }
        public void PasswordEntryChanged()
        {
            if (!IsValidPassword(Password))
            {
                PasswordEntryColor = "Red";
                NotifyLabel = "Invalid password format! Password must contain: \n·At least 8 characters\n·One uppercase letter\n·One lowercase letter\n·One digit\n·One special character";
            }
            else
            {
                PasswordEntryColor = "Green";
                NotifyLabel = string.Empty;
            }
        }
        public void EmailEntryChanged()
        {
            if (!IsValidEmail(Email))
            {
                EmailEntryColor = "Red";
                NotifyLabel = "Invalid email format!";
            }
            else
            {
                EmailEntryColor = "Green";
                NotifyLabel = string.Empty;
            }
        }
        public void NicknameEntryChanged()
        {
            if (!IsValidNickname(NickName))
            {
                EmailEntryColor = "Red";
                NotifyLabel = "Invalid nickname format! Nickname must contain: \n·At least 3 characters\n·Only letters,digits and _";
            }
            else
            {
                EmailEntryColor = "Green";
                NotifyLabel = string.Empty;
            }
        }
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            return Regex.IsMatch(email, pattern);
        }
        private bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }
        private bool IsValidNickname(string nickname)
        {
            if (nickname.Length < 3 || nickname.Length > 18)
            {
                return false;
            }
            foreach (char c in nickname)
            {
                if (!char.IsLetterOrDigit(c) && c != '_')
                {
                    return false;
                }
            }
            return true;
        }

        public async Task GoBack()
        {
            Email = string.Empty;
            Password = string.Empty;
            NickName = string.Empty;
            NotifyLabel = string.Empty;
            await Shell.Current.GoToAsync($"///{nameof(LogInPage)}");
        }
    }
}
