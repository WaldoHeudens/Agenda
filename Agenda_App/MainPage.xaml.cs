using Agenda_App.ViewModels;

namespace Agenda_App
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel _viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel;
        }
    }
}
