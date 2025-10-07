using Agenda_Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Agenda_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AgendaDbContext context = new AgendaDbContext();
            //dgAppointments.ItemsSource = context.Appointments
            //                                    .Where(app => app.Deleted >= DateTime.Now
            //                                                    && app.From > DateTime.Now)
            //                                    .Include(app => app.AppointmentType)  // Eager loading van AppointmentType
            //                                    .ToList();
            dgAppointments.ItemsSource = (from app in context.Appointments
                                          where app.Deleted >= DateTime.Now
                                                  && app.From > DateTime.Now
                                          orderby app.From
                                          select new Appointment{   Id = app.Id,
                                                                    From=app.From, 
                                                                    To= app.To, 
                                                                    Title=app.Title, 
                                                                    Description = app.Description, 
                                                                    AllDay = app.AllDay, 
                                                                    AppointmentType= app.AppointmentType })
                                        //.Include(app => app.AppointmentType)  // Eager loading van AppointmentType
                                        .ToList();
        }

        private void dgAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grDetails.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Hidden;
            if (dgAppointments.SelectedIndex == dgAppointments.Items.Count - 1)
            {
                btnEdit.IsEnabled = false;  
                btnDelete.IsEnabled = false;
            }
            else
            {
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            grDetails.Visibility = Visibility.Visible;
            grDetails.DataContext = dgAppointments.SelectedItem;
            btnSave.Visibility = Visibility.Hidden;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            AgendaDbContext context = new AgendaDbContext();
            try
            {
                Appointment appointment = (Appointment)dgAppointments.SelectedItem;
                Appointment contextAppointment = context.Appointments
                                                            .FirstOrDefault(app => app.Id == appointment.Id);
                if (contextAppointment != null)
                {
                    contextAppointment.From = appointment.From;
                    contextAppointment.To = appointment.To;
                    contextAppointment.Title = appointment.Title;
                    contextAppointment.Description = appointment.Description;
                    contextAppointment.AllDay = appointment.AllDay;
                    contextAppointment.AppointmentTypeId = appointment.AppointmentTypeId;
                    context.SaveChanges();
                }
            }
            catch
            {
                Appointment appointment = (Appointment) grDetails.DataContext;
                context.Appointments.Add(appointment);
                context.SaveChanges();

                dgAppointments.ItemsSource = (from app in context.Appointments
                                              where app.Deleted >= DateTime.Now
                                                      && app.From > DateTime.Now
                                              orderby app.From
                                              select new Appointment
                                              {
                                                  Id = app.Id,
                                                  From = app.From,
                                                  To = app.To,
                                                  Title = app.Title,
                                                  Description = app.Description,
                                                  AllDay = app.AllDay,
                                                  AppointmentType = app.AppointmentType
                                              })
                                            //.Include(app => app.AppointmentType)  // Eager loading van AppointmentType
                                            .ToList();
            }
            btnSave.Visibility = Visibility.Hidden;

        }

        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            btnSave.Visibility = Visibility.Visible;
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSave.Visibility = Visibility.Visible;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            btnSave.Visibility = Visibility.Hidden;
            grDetails.Visibility = Visibility.Visible;
            grDetails.DataContext = new Appointment();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            AgendaDbContext context = new AgendaDbContext();
            Appointment appointment = (Appointment)dgAppointments.SelectedItem;
            Appointment contextAppointment = context.Appointments
                                                        .FirstOrDefault(app => app.Id == appointment.Id);
            if (contextAppointment != null)
            {
                contextAppointment.Deleted = DateTime.Now;
                context.SaveChanges();

                dgAppointments.ItemsSource = (from app in context.Appointments
                                              where app.Deleted >= DateTime.Now
                                                      && app.From > DateTime.Now
                                              orderby app.From
                                              select new Appointment
                                              {
                                                  Id = app.Id,
                                                  From = app.From,
                                                  To = app.To,
                                                  Title = app.Title,
                                                  Description = app.Description,
                                                  AllDay = app.AllDay,
                                                  AppointmentType = app.AppointmentType
                                              })
                                            //.Include(app => app.AppointmentType)  // Eager loading van AppointmentType
                                            .ToList();

            }
        }
    }
}