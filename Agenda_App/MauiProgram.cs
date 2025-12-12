using Agenda_App.Pages;
using Agenda_App.Services;
using Agenda_Models;
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

            // Configureer logging via de API naar de databank
            builder.Logging.AddDbLogger(options =>
                {
                    builder.Configuration
                        .GetSection("Logging");
                }
            );

            // Registreren van de dbContext als service
            builder.Services.AddDbContext<LocalDbContext>();

            // Toegevoegd als (Dependency Injection) service voor LoginPage en LoginViewModel
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<ViewModels.LoginViewModel>();

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
