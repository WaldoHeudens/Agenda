using Agenda_App.ViewModels;
using Agenda_Models;
using SQLitePCL;

namespace Agenda_App.Pages;

public partial class LoginPage : ContentPage
{
	readonly LocalDbContext _context;

	public LoginPage(LoginViewModel viewModel, LocalDbContext context)
	{
		_context = context;
		InitializeComponent();
		BindingContext = viewModel;
	}
}