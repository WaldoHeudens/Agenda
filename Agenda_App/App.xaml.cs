using Agenda_Models;

namespace Agenda_App
{
    public partial class App : Application
    {
        public App(LocalDbContext context)
        {
            // Zorg ervoor dat de database is geïnitialiseerd en de autorisatie is gecontroleerd
            // voordat de UI wordt geladen

            Synchronizer s = new Synchronizer(context);
            s.InitializeDb();
            s.SynchronizeAll();
            do
            {
                Thread.Sleep(10);
            } while (!s.dbExists);

            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}