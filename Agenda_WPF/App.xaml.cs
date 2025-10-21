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
        static public AgendaUser User { get; set; }
        static public MainWindow MainWindow { get; set; }

        static public string ConnectionString = "";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Setup Dependency Injection
            var services = new ServiceCollection();

            // Setup DbContext als Service
            services.AddDbContext<AgendaDbContext>();

            // Voeg Identity framework toe als service
            services.AddIdentityCore<AgendaUser>() // Use AddIdentityCore instead of AddIdentity
                .AddRoles<IdentityRole>()          // Add roles support
                .AddEntityFrameworkStores<AgendaDbContext>();

            services.AddLogging();

            // Creëer de ServiceProvider die overal toegangkelijk zal zijn
            ServiceProvider = services.BuildServiceProvider();

            AgendaDbContext context = new AgendaDbContext();
            AgendaDbContext.Seeder(context);

            App.User = AgendaUser.dummy;

            MainWindow = new MainWindow(App.ServiceProvider.GetRequiredService<AgendaDbContext>());   
            MainWindow.Show();
        }
    }

}
