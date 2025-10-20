using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Agenda_WPF
{

    // Onze eigen ColorPicker-klasse
    public class ColorPicker : Control
    {
        private Rectangle _rect;        // De ColorPicker rechthoek die de kleur toont is als er geen "activiteit" is
        private Popup _popup;           // Het popup-window van de ColorPicker al er een muisklik is op de rechthoek
        private ItemsControl _items;    // De lijst met kleuren (Brushes) waaruit gekozen kan worden

        private const string Part_Rect = "Part_Rectangle";  // De externe naam van de rechthoek
        private const string Part_Popup = "Part_Popup";     // De externe naam van het popup-window
        private const string Part_Items = "Part_Items";     // De externe naam van de list van kleuren

        public IEnumerable<Brush> PredefinedBrushes         // De verzameling van alle voorgedefinieerde kleuren (Brushes)
        {
            get => (IEnumerable<Brush>)GetValue(PredefinedBrushesProperty);
            set => SetValue(PredefinedBrushesProperty, value);
        }



        static ColorPicker()  // Statische constructor die de meta-data definieerd
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        public ColorPicker()   // Constructor
        {
            PredefinedBrushes = DefaultBrushes;     // Zorg dat onze lijst van kleuren in de voorgedefinieerde kleuren zit
        }

        // Registratie van eigenschappen die zichtbaar zijn voor de buitenwereld:
        // - DefaultBrushes
        // - SelectedColor

        public static readonly DependencyProperty PredefinedBrushesProperty =
             DependencyProperty.Register(nameof(PredefinedBrushes), typeof(IEnumerable<Brush>),
                 typeof(ColorPicker), new PropertyMetadata(DefaultBrushes));  

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(ColorPicker),
                new FrameworkPropertyMetadata(Colors.Transparent, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));

        private static readonly SolidColorBrush[] DefaultBrushes = new SolidColorBrush[]  // Een array met de voorgedefinieerde kleuren
{
                Brushes.Black,
                Brushes.White,
                Brushes.Gray,
                Brushes.Red,
                Brushes.Orange,
                Brushes.Yellow,
                Brushes.Green,
                Brushes.Blue,
                Brushes.Indigo,
                Brushes.Magenta,
                Brushes.Brown,
                Brushes.Pink,
                Brushes.Transparent
};

        public Color SelectedColor
        {
            get => (Color)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        // Registratie van eventhandlers die zichtbaar moeten zijn 
        // - SelectedColorChangedEvent en overeenkomstige delegate definitie SelectedColorChanged
        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker picker = (ColorPicker)d;
            picker.UpdateRectangle();
            picker.RaiseEvent(new RoutedEventArgs(SelectedColorChangedEvent));
        }

        public static readonly RoutedEvent SelectedColorChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(SelectedColorChanged), RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(ColorPicker));


        public event RoutedEventHandler SelectedColorChanged
        {
            add => AddHandler(SelectedColorChangedEvent, value);
            remove => RemoveHandler(SelectedColorChangedEvent, value);
        }


        // Methode die wordt uitgevoerd als een kleur gekozen wordt (met open popup)
        private void UpdateRectangle()
        {
            if (_rect != null)
            {
                _rect.Fill = new SolidColorBrush(SelectedColor);
            }
        }

        // Eventhandler die uitgevoerd wordt als een item wordt gekozen (linker muisknop loslaten):
        //   Sluit de popup nadat de kleur is toegekend aan SelectedColor, zowel bij klik op het kleurveld, als op de border van het kleurveld

        private void Items_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle rect && rect.Fill is SolidColorBrush solidColorBrush)
            {
                SelectedColor = solidColorBrush.Color;
                _popup.IsOpen = false;
            }
            else if (e.OriginalSource is Border border && border.Background is SolidColorBrush borderSolidColorBrush2)
            {
                SelectedColor = borderSolidColorBrush2.Color;
                _popup.IsOpen = false;
            }
        }


        // Eventhandler die wordt uitgevoerd als op de zichtbare rechthoek wordt geklikt (muis losgelaten): Open of sluit de popup
        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_popup != null)
            {
                _popup.IsOpen = !_popup.IsOpen;
            }
        }


        // Methode die wordt uitgevoerd door InitializeComponent om de ColorPicker te interpreteren
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_rect != null)
            {
                // Start met een lege lijst van de eventhandler
                _rect.MouseLeftButtonUp -= Rectangle_MouseLeftButtonUp;
            }

            // Definieer de 3 onderdelen
            _rect = (Rectangle)GetTemplateChild(Part_Rect);
            _popup = (Popup)GetTemplateChild(Part_Popup);
            _items = (ItemsControl)GetTemplateChild(Part_Items);

            if (_rect != null)
            {
                // Initialiseer de rechthoek in de muis-eventhandler voor de rechthoek
                UpdateRectangle();
                _rect.Cursor = Cursors.Hand;
                _rect.MouseLeftButtonUp += _rect_MouseLeftButtonUp;
            }

            if (_items != null)
            {
                // Initializeer de items, en de eventhandlers van de items
                _items.ItemsSource = PredefinedBrushes;
                _items.MouseLeftButtonUp -= Items_MouseLeftButtonUp;
                _items.MouseLeftButtonUp += _items_MouseLeftButtonUp;
            }
        }

        // De eigenlijke eventhandler voor de kleurselectie
        private void _items_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Clicking a swatch rectangle populates SelectedColor and closes popup
            if (e.OriginalSource is Rectangle rect && rect.Fill is SolidColorBrush scb)
            {
                SelectedColor = scb.Color;
                _popup.IsOpen = false;
            }
            else if (e.OriginalSource is Border border && border.Background is SolidColorBrush scb2)
            {
                SelectedColor = scb2.Color;
                _popup.IsOpen = false;
            }
        }

        // De eigenlijke eventhandler voor het openen van de popup
        private void _rect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_popup != null)
            {
                _popup.IsOpen = !_popup.IsOpen;
            }
        }
    }
}
