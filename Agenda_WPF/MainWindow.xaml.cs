using Agenda_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly AgendaDbContext _context;

        public MainWindow(AgendaDbContext context)
        {
            _context = context;

            InitializeComponent();
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
            try
            {
                Appointment appointment = (Appointment)dgAppointments.SelectedItem;
                Appointment contextAppointment = _context.Appointments.FirstOrDefault(app => app.Id == appointment.Id);
                if (contextAppointment != null)
                {
                    contextAppointment.UserId = App.User.Id;
                    contextAppointment.From = appointment.From;
                    contextAppointment.To = appointment.To;
                    contextAppointment.Title = appointment.Title;
                    contextAppointment.Description = appointment.Description;
                    contextAppointment.AllDay = appointment.AllDay;
                    contextAppointment.AppointmentTypeId = appointment.AppointmentType.Id;
                    _context.SaveChanges();
                }
            }
            catch
            {
                Appointment appointment = (Appointment) grDetails.DataContext;
                appointment.UserId= App.User.Id;
                _context.Appointments.Add(appointment);
                _context.SaveChanges();

                dgAppointments.ItemsSource = (from app in _context.Appointments
                                              where app.Deleted >= DateTime.Now
                                                      && app.From > DateTime.Now
                                                       && app.UserId == App.User.Id
                                              orderby app.From
                                              select app)
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
            Appointment appointment = (Appointment)dgAppointments.SelectedItem;
            Appointment contextAppointment = _context.Appointments.FirstOrDefault(app => app.Id == appointment.Id);
            if (contextAppointment != null)
            {
                contextAppointment.Deleted = DateTime.Now;
                _context.SaveChanges();

                dgAppointments.ItemsSource = (from app in _context.Appointments
                                              where app.Deleted >= DateTime.Now
                                                      && app.From > DateTime.Now
                                                      && app.UserId == App.User.Id
                                              orderby app.From
                                              select app)
                                            //.Include(app => app.AppointmentType)  // Eager loading van AppointmentType
                                            .ToList();

            }
        }

        private void mniRegister_Click(object sender, RoutedEventArgs e)
        {
            new RegisterWindow(_context, App.ServiceProvider.GetRequiredService<UserManager<AgendaUser>>()).ShowDialog();
        }

        private void mniLogin_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow(_context, App.ServiceProvider.GetRequiredService<UserManager<AgendaUser>>()).ShowDialog();
            if (App.User.Id != AgendaUser.dummy.Id)
            {
                // Bij login moeten de afspraken opnieuw opgehaald worden voor deze gebruiker
                dgAppointments.ItemsSource = (from app in _context.Appointments
                                              where app.Deleted >= DateTime.Now
                                                      && app.From > DateTime.Now
                                                      && app.UserId == App.User.Id
                                              orderby app.From
                                              select app)
                                            //.Include(app => app.AppointmentType)  // Eager loading van AppointmentType
                                            .ToList();
                tbFilter.Text = "";
                cbTypes.ItemsSource = _context.AppointmentTypes
                                        .Where(apt => apt.Deleted >= DateTime.Now
                                                    && apt.UserId == App.User.Id)
                                        .ToList();
                dgAppointments.Visibility = Visibility.Visible;
                spGeneral.Visibility = Visibility.Visible;
                mniTypes.Visibility = Visibility.Visible;
                tbFilter.Visibility = Visibility.Visible;
            }
        }

        private void mniLogout_Click(object sender, RoutedEventArgs e)
        {
            mnUserKnow.Visibility = Visibility.Collapsed;
            mnNoUser.Visibility = Visibility.Visible;
            mnUsers.Visibility = Visibility.Collapsed;
            dgAppointments.Visibility = Visibility.Collapsed;
            tbFilter.Visibility = Visibility.Collapsed;
            spGeneral.Visibility = Visibility.Collapsed;
            mniTypes.Visibility = Visibility.Collapsed;
            mniSystem.Visibility = Visibility.Collapsed;
            App.User = AgendaUser.dummy;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Roles_Click(object sender, RoutedEventArgs e)
        {
            new RolesWindow(_context, App.ServiceProvider.GetRequiredService<UserManager<AgendaUser>>()).ShowDialog();
        }

        private void mniTypes_Click(object sender, RoutedEventArgs e)
        {
            (new TypeWindow(_context, App.User.Id)).ShowDialog();
        }

        private void mniSystemTypes_Click(object sender, RoutedEventArgs e)
        {
            (new TypeWindow(_context, AgendaUser.dummy.Id)).ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            (new TypeWindow(_context, App.User.Id)).ShowDialog();
        }

        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
           dgAppointments.ItemsSource = (from app in _context.Appointments
                                          where app.Deleted >= DateTime.Now
                                                  && app.From > DateTime.Now
                                                  && app.UserId == App.User.Id
                                                  && (tbFilter.Text.Length==0 
                                                        || (app.Title.Contains(tbFilter.Text)   
                                                            || app.Description.Contains(tbFilter.Text)))
                                          orderby app.From
                                          select app)
                            //.Include(app => app.AppointmentType)  // Eager loading van AppointmentType
                            .ToList();
        }
    }
}