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
            InitializeComponent();
            BindingContext = _viewModel;
        }


        //private void Bewerkt_Invoked(object sender, EventArgs e)
        //{

        //}
    }
}
