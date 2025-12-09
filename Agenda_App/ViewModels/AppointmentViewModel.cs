using Agenda_Cons.Migrations;
using Agenda_Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda_App.ViewModels
{
    public partial class AppointmentViewModel : ObservableObject
    {
        readonly LocalDbContext _context;
        public AppointmentViewModel(Appointment appointment, LocalDbContext context)
        { 
            _context = context;
            this.appointment = appointment;
            Title = appointment.Title;
            Description = appointment.Description;
            From = appointment.From;
            To = appointment.To;
            AllDay = appointment.AllDay;
            appointmentTypes = _context.LocalAppointmentTypes.ToList();
        }

        [ObservableProperty]
        Appointment appointment;

        [ObservableProperty]
        DateTime from;

        [ObservableProperty]
        DateTime to;

        [ObservableProperty]
        string title;

        [ObservableProperty]
        string description;

        [ObservableProperty]
        bool allDay;

        [ObservableProperty]
        List<AppointmentType> appointmentTypes;

        [ObservableProperty]
        AppointmentType selectedAppointmentType;


        [RelayCommand]
        async Task Save()
        {
            appointment.Title = Title;
            appointment.Description = Description;
            appointment.From = From;
            appointment.To = To;
            appointment.AllDay = AllDay;
            appointment.Deleted = General.Dirty; // Mark as dirty for synchronization
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
