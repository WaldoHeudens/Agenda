using Agenda_App.ViewModels;

namespace Agenda_App.Pages;

public partial class AppointmentPage : ContentPage
{
	public AppointmentPage(AppointmentViewModel _viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel;
    }
}