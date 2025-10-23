using Agenda_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
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
        List<AppointmentType> changedObjects = new List<AppointmentType>();
        string _userId;

        private readonly AgendaDbContext _context;

        public TypeWindow(AgendaDbContext context, string userId)
        {
            _context = context;
            _userId = userId;

            InitializeComponent();

            List<TypeViewModel> types = (from type in _context.AppointmentTypes
                                            where type.UserId == userId
                                                    && type.Deleted > DateTime.Now
                                            select new TypeViewModel(type))
                                            .ToList();
            dgTypes.ItemsSource = types;
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
            changedObjects.Add (((TypeViewModel) dgTypes.SelectedItem).Type);
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


        private void ColorPicker_SelectedColorChanged(object sender, RoutedEventArgs e)
        {
            btnSave.IsEnabled = true;
            TypeViewModel tvm = (TypeViewModel)dgTypes.SelectedItem;
            tvm.Type.Color = tvm.Brush.Color.ToString();
            changedObjects.Add(tvm.Type);
        }
    }

    // Een "ViewModel" om de binding te maken met een SolidColorBrush, i.p.v. de string Color
    class TypeViewModel
    { 
        public AppointmentType Type { get; set; }
        public SolidColorBrush Brush { get; set; }

        public TypeViewModel(AppointmentType type)
        {
            Type = type;
            if (!Type.Color.StartsWith("#"))
                Type.Color = type.Color.Insert(0, "#");
            Color c = (Color)ColorConverter.ConvertFromString(Type.Color);
            Brush = new SolidColorBrush(c);
        }
    }
}
