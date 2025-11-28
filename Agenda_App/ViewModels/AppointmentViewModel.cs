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

        public AppointmentViewModel(Appointment appointment)
        { 
            this.appointment = appointment;
            Title = appointment.Title;
            Description = appointment.Description;
            From = appointment.From;
            To = appointment.To;
            AllDay = appointment.AllDay;
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


        [RelayCommand]
        async Task Save()
        {
            appointment.Title = Title;
            appointment.Description = Description;
            appointment.From = From;
            appointment.To = To;
            appointment.AllDay = AllDay;
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
