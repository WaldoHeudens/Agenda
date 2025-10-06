using Agenda_Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Agenda_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AgendaDbContext context = new AgendaDbContext();
            AgendaDbContext.Seeder(context);

            // Wordt automatisch uitgevoerd door App.xaml
            //MainWindow mainWindow = new MainWindow();   
            //mainWindow.Show();
        }
    }

}
