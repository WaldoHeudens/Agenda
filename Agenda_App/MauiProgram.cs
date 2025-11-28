using Microsoft.Extensions.Logging;

namespace Agenda_App
{
    public static class MauiProgram
    {
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
            // Toegevoegd als (Dependency Injection) service voor MainPage en MainViewModel
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ViewModels.MainViewModel>();

            // Toegevoegd als (Dependency Injection) service voor AppointmentPage en AppointmentViewModel
            builder.Services.AddTransient<Pages.AppointmentPage>();
            builder.Services.AddTransient<ViewModels.AppointmentViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
