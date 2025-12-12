using Agenda_App.Pages;
using Agenda_Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Collections.ObjectModel;

namespace Agenda_App.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        LocalDbContext _context;

        public MainViewModel(LocalDbContext context)
        {
            _context = context;
            appointments = [.. _context.Appointments.Where(a => a.Deleted > DateTime.Now)];
        }

        [ObservableProperty]
        ObservableCollection<LocalAppointment> appointments;


        [ObservableProperty]
        string wat = "";

        [ObservableProperty]
        string wanneer = "";


        [RelayCommand]
        void VoegToe()
        {
            try
            {
                // Voeg een nieuwe afspraak toe met (voorlopige?) vaste waarden
                LocalAppointment app = new LocalAppointment();
                app.Id = General.LocalIdCounter--;  // een unieke negatieve ("dirty"!) Id
                app.Title = Wat;
                app.From = DateTime.Parse(Wanneer);
                app.To = DateTime.Parse(Wanneer).AddHours(1);
                app.Created = DateTime.Now;
                app.AppointmentType = _context.AppointmentTypes.First(); // Standaard type
                Wat = string.Empty;
                Wanneer = string.Empty;
                app.User = _context.Users.First(); // Standaard gebruiker
                _context.Appointments.Add(app);
                _context.SaveChanges();
                appointments.Add(app);
            }
            catch 
            {
                // Handle parsing error (e.g., show a message to the user)
            }
        }

        [RelayCommand]
        async void Verwijder(Appointment appointment)
        {
            if (await Application.Current.MainPage.DisplayAlert("Verwijder Afspraak", "Ben je zeker dat je deze afspraak wil verwijderen?", "Ja", "Nee"))
            {
                appointment.Deleted = DateTime.Now;
                _context.Update(appointment);
                _context.SaveChanges();
                Appointments = [.. _context.Appointments.Where(a => a.Deleted > DateTime.Now)];
            }
        }

        [RelayCommand]
        async void Bewerk(LocalAppointment appointment)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AppointmentPage(new AppointmentViewModel(appointment, _context), _context));
            Appointments = [.. _context.Appointments.Where(a => a.Deleted > DateTime.Now)];
        }
    }
}
