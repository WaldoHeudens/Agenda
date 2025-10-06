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
            dgAppointments.ItemsSource = context.Appointments
                                                .Where(app => app.Deleted >= DateTime.Now
                                                                && app.From > DateTime.Now)
                                                .Include(app => app.AppointmentType)  // Eager loading van AppointmentType
                                                .ToList();
        }
    }
}