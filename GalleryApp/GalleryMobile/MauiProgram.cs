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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton(cancellationTokenSource);

            builder.Services.AddSingleton<UnsplashAPIClient>();
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<ImageDetialsViewModel>(p => new ImageDetialsViewModel(new UnsplashAPI.Model.UnsplashPhoto
            {
                /* NOTE: Initialize UnsplashPhoto object with random data to inject */
                Id = "1",
                Description = "asdasdasd",
                Url = new Uri("https://images.unsplash.com/face-springmorning.jpg?q=75&fm=jpg&w=1080&fit=max")
            }));


            builder.Services.AddView<MainPage, MainPageViewModel>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void AddView<TView, TViewModel>(this IServiceCollection services)
            where TView : Element, new()
            where TViewModel : notnull
        {
            services.AddSingleton<TView>(serviceProvider => new TView()
            {
                BindingContext = serviceProvider.GetRequiredService<TViewModel>()
            });
        }

    }
}
