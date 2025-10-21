using Agenda_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Agenda_WPF
{
    /// <summary>
    /// Interaction logic for TypeWindow.xaml
    /// </summary>
    public partial class TypeWindow : Window
    {
        
        AgendaDbContext _context;
        List<AppointmentType> changedObjects = new List<AppointmentType>();
        string _userId;

        Menu TypesContextMenu = new Menu();

        public TypeWindow(AgendaDbContext context, string userId)
        {
            _context = context;
            _userId = userId;
            InitializeComponent();

            List<AppointmentType> types = _context.AppointmentTypes
                                            .Where(at => at.UserId==userId
                                                    && at.Deleted > DateTime.Now)
                                            .ToList();
            dgTypes.ItemsSource = types;

            TypesContextMenu.Items.Add(new TypeWindow(_context, App.User.Id));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (AppointmentType type in changedObjects)
            {
                type.UserId = _userId;
                if (type.Id > 0)
                    _context.Update(type);
                else
                    _context.Add(type);
            }
            _context.SaveChanges();
            btnSave.IsEnabled = false;
            btnDelete.IsEnabled = false;
        }

        private void dgTypes_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            btnSave.IsEnabled = true;
            changedObjects.Add ((AppointmentType) dgTypes.SelectedItem);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Wil je dit type echt verwijderen?","Type verwijderen", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ((AppointmentType) dgTypes.SelectedItem).Deleted = DateTime.Now;
                _context.SaveChanges();   // Opgepast: Alle wijzigingen worden bewaard zonder te bevestigen !
                btnSave.IsEnabled=false;
                List<AppointmentType> types = _context.AppointmentTypes
                                                .Where(at => at.UserId == _userId
                                                        && at.Deleted > DateTime.Now)
                                                .ToList();
                dgTypes.ItemsSource = types;
            }
            btnDelete.IsEnabled = false;
        }

        private void dgTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDelete.IsEnabled = true;
        }

        private void cpTest_SelectedColorChanged(object sender, RoutedEventArgs e)
        {
            //rectPicker.Fill = new SolidColorBrush(cpTest.SelectedColor);
        }
    }
}
