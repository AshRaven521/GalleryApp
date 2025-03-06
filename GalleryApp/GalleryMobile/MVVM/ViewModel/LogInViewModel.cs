using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GalleryMobile.DataPersistence.Services;
using GalleryMobile.MVVM.View.Pages;

namespace GalleryMobile.MVVM.ViewModel
{
    public partial class LogInViewModel : ObservableObject, IQueryAttributable
    {
        private string email = string.Empty;
        private string password = string.Empty;
        private string notifyLabel = string.Empty;
        private bool isFirstAppearing = true;
        private readonly IGalleryAppDatabaseService database;
        private readonly CancellationTokenSource cancellationTokenSource;

        public LogInViewModel(IGalleryAppDatabaseService database, CancellationTokenSource cancellationTokenSource)
        {
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
        [RelayCommand]
        public async Task LogInAsync()
        {
            await LogIn();
        }
        [RelayCommand]
        public async Task RegisterAsync()
        {
            await GoToRegisterPage();
        }

        [RelayCommand]
        public async Task PageAppearingAsync()
        {
            /* NOTE: If page appears first time check for logged in user. If page appears after redirect from another page skip this step */
            if (isFirstAppearing)
            {
                var user = await database.GetLastLoggedInUser(cancellationTokenSource.Token);
                if (user != null)
                {
                    var navigationParameter = new Dictionary<string, object>
                    {
                        {"CurrentUser", user }
                    };

                    await Shell.Current.GoToAsync(nameof(MainPage), navigationParameter);
                }

            }
        }

        public async Task LogIn()
        {
            if (Email == string.Empty)
            {
                NotifyLabel = "Email is empty!";
                return;
            }
            if (Password == string.Empty)
            {
                NotifyLabel = "Password in empty!";
                return;
            }
            var user = await database.GetUserByEmailAsync(Email, cancellationTokenSource.Token);

            if (user != null)
            {
                bool isPasswordVerified = BCrypt.Net.BCrypt.Verify(Password, user.Password);
                if (!isPasswordVerified)
                {
                    NotifyLabel = "Incorrect email or password!";
                    return;
                }

                user.IsLoggedIn = true;
                await database.SaveUserAsync(user, cancellationTokenSource.Token);

            }
            else
            {
                NotifyLabel = "Incorrect email or password!";
                return;
            }


            Email = string.Empty;
            Password = string.Empty;
            NotifyLabel = string.Empty;

            var navigationParameter = new Dictionary<string, object>
            {
                {"CurrentUser", user }
            };

            await Shell.Current.GoToAsync(nameof(MainPage), navigationParameter);
        }

        public async Task GoToRegisterPage()
        {
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            isFirstAppearing = (bool)query["IsFirstLoad"];
        }
    }
}
