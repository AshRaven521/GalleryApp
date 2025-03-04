using CommunityToolkit.Maui;
using GalleryMobile.DataPersistence;
using GalleryMobile.DataPersistence.Services;
using GalleryMobile.MVVM.View.Pages;
using GalleryMobile.MVVM.ViewModel;
using GalleryMobile.UnsplashAPI;
using Microsoft.Extensions.Logging;


namespace GalleryMobile
{
    public static class MauiProgram
    {
        // Will dispose after application closed ( MAUI dispose services before finish and cancellationTokenSource register in services as singleton(for all app life) )
        private static readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddDbContext<GalleryAppContext>();

            builder.Services.AddSingleton(cancellationTokenSource);

            builder.Services.AddSingleton<IUnsplashAPIClient, UnsplashAPIClient>();
            builder.Services.AddSingleton<IGalleryAppDatabaseService, GalleryAppDatabaseService>();


            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddSingleton<ImageDetialsViewModel>();

            builder.Services.AddSingleton<RegistrationViewModel>();

            builder.Services.AddSingleton<LogInViewModel>();


            builder.Services.AddView<MainPage, MainPageViewModel>();

            builder.Services.AddView<ImageDetailsPage, ImageDetialsViewModel>();

            builder.Services.AddView<RegistrationPage, RegistrationViewModel>();
            builder.Services.AddView<LogInPage, LogInViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void AddView<TView, TViewModel>(this IServiceCollection services)
            where TView : ContentPage, new()
            where TViewModel : notnull
        {
            services.AddSingleton<TView>(serviceProvider => new TView()
            {
                BindingContext = serviceProvider.GetRequiredService<TViewModel>()
            });
        }

    }
}
