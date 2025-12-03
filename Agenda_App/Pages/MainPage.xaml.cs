using Agenda_App.ViewModels;
using Agenda_Models;
using Microsoft.EntityFrameworkCore;

namespace Agenda_App
{
    public partial class MainPage : ContentPage
    {
        readonly LocalDbContext _context; // Toegevoegd om de database context op te slaan

        public MainPage(MainViewModel _viewModel, LocalDbContext context)
        {
            _context = context; // Initialiseer de database context
            InitializeDb();
            InitializeComponent();
            BindingContext = _viewModel;
        }

        private void InitializeDb()
        {
            // Zorg ervoor dat de database is aangemaakt en de laatste migraties zijn toegepast
            _context.Database.MigrateAsync();


            // Zolang er nog geen synchronisatie is geweest, voeg een basis AppointmentType toe
            if (!_context.AppointmentTypes.Any())
            {
                Language nederlands = new Language { Code = "nl", Name = "Nederlands" };
                _context.Languages.Add(nederlands);
                AgendaUser user = new AgendaUser { UserName = "LocalUser", Email = "(local)", FirstName = "Local", LastName = "User", Language=nederlands };
                _context.Users.Add(user);
                _context.SaveChanges();
                _context.AppointmentTypes.Add(new AppointmentType { Name = "?", User=user });
                _context.SaveChanges();
            }
        }

        //private void Bewerkt_Invoked(object sender, EventArgs e)
        //{

        //}
    }
}
