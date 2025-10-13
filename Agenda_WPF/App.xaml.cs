using Agenda_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Agenda_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        static public ServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Setup Dependency Injection
            var services = new ServiceCollection();

            // Setup DbContext als Service
            services.AddDbContext<AgendaDbContext>();

            // Voeg Identity framework toe als service
            services.AddIdentityCore<AgendaUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
            }).AddEntityFrameworkStores<AgendaDbContext>();


            services.AddLogging();

            // Creëer de ServiceProvider die overal toegangkelijk zal zijn
            ServiceProvider = services.BuildServiceProvider();


            AgendaDbContext context = new AgendaDbContext();
            AgendaDbContext.Seeder(context);

            MainWindow mainWindow = new MainWindow(App.ServiceProvider.GetRequiredService<AgendaDbContext>());   
            mainWindow.Show();
        }
    }

}
