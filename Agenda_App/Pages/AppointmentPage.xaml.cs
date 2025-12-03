using Agenda_App.ViewModels;
using Agenda_Models;

namespace Agenda_App.Pages;

public partial class AppointmentPage : ContentPage
{
    readonly LocalDbContext _context; // Toegevoegd om de database context op te slaan  
    public AppointmentPage(AppointmentViewModel _viewModel, LocalDbContext context)
	{
        _context = context; // Initialiseer de database context

        InitializeComponent();
		BindingContext = _viewModel;
    }
}