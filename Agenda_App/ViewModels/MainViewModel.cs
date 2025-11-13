using Agenda_Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Agenda_App.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            appointments = new ObservableCollection<Appointment>();
        }

        [ObservableProperty]
        ObservableCollection<Appointment> appointments = new ObservableCollection<Appointment>();


        [ObservableProperty]
        string wat = "";

        [ObservableProperty]
        string wanneer = "";


        [RelayCommand]
        void VoegToe()
        {
            try
            {
                Appointment app = new Appointment();
                app.Title = Wat;
                app.From = DateTime.Parse(Wanneer);
                app.To = DateTime.Parse(Wanneer).AddHours(1);
                app.Created = DateTime.Now;
                Wat = string.Empty;
                Wanneer = string.Empty;
                appointments.Add(app);
            }
            catch(Exception ex)
            {
                // Handle parsing error (e.g., show a message to the user)
            }
        }
    }
}
