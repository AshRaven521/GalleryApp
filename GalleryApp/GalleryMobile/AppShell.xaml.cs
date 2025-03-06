using GalleryMobile.MVVM.View.Pages;

namespace GalleryMobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(ImageDetailsPage), typeof(ImageDetailsPage));
        }
    }
}
