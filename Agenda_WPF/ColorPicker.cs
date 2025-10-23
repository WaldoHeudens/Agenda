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


    [TemplatePart(Name = CP_Rectangle, Type = typeof(Rectangle))]
    [TemplatePart(Name = CP_Popup, Type = typeof(Popup))]
    [TemplatePart(Name = CP_Items, Type = typeof(ItemsControl))]

    public class ColorPicker : Control
    {
        private Rectangle _rect;        // De ColorPicker rechthoek die de kleur toont is als er geen "activiteit" is
        private Popup _popup;           // Het popup-window van de ColorPicker al er een muisklik is op de rechthoek
        private ItemsControl _items;    // De lijst met kleuren (Brushes) waaruit gekozen kan worden

        private const string CP_Rectangle = "CP_Rectangle";  // De externe naam van de rechthoek
        private const string CP_Popup = "CP_Popup";         // De externe naam van het popup-window
        private const string CP_Items = "CP_Items";         // De externe naam van de list van kleuren

        // Een array met de voorgedefinieerde kleuren:
        private static readonly SolidColorBrush[] DefaultBrushes = new SolidColorBrush[]  
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


        public IEnumerable<SolidColorBrush> PredefinedBrushes         // De verzameling van alle voorgedefinieerde kleuren (Brushes)
        {
            get => (IEnumerable<SolidColorBrush>)GetValue(PredefinedBrushesProperty);
            set => SetValue(PredefinedBrushesProperty, value);
        }

        public SolidColorBrush SelectedColor
        {
            get => (SolidColorBrush)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        // Registratie van eigenschappen die zichtbaar zijn voor de buitenwereld:
        // - DefaultBrushes
        // - SelectedColor
        public static readonly DependencyProperty PredefinedBrushesProperty =
                DependencyProperty.Register(nameof(PredefinedBrushes), typeof(IEnumerable<SolidColorBrush>), typeof(ColorPicker),
                new PropertyMetadata(DefaultBrushes));

        public static readonly DependencyProperty SelectedColorProperty =
                DependencyProperty.Register(nameof(SelectedColor), typeof(SolidColorBrush), typeof(ColorPicker),
                new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));


        static ColorPicker()  // Statische constructor die zorgt dat onze ColorPicker gekend is als type
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        public ColorPicker()   // Constructor
        {
            PredefinedBrushes = DefaultBrushes;     // Zorg dat onze lijst van kleuren in de voorgedefinieerde verzameling kleuren zit
        }




        // De eventhandler die de logica behandelt bij het klikken op een kleur
        public static readonly RoutedEvent SelectedColorChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(SelectedColorChanged), RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(ColorPicker));

        private static void OnSelectedColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker picker = (ColorPicker) o;   // haal de instantie op van de aangeklikte ColorPicker
            picker.UpdateRectangle();               // toon of verwijder de rechthoek met de kleurenkeuze
            //picker.RaiseEvent(new RoutedEventArgs(SelectedColorChangedEvent));  // start het event met de uitvoering van (een) gebruikers-delegate(s)
        }

        // Toevoegen of verwijderen van een gebruikers-delegate
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
                _rect.Fill = SelectedColor;
            }
        }

        // Eventhandler die uitgevoerd wordt als een item wordt gekozen (linker muisknop loslaten):
        //   Sluit de popup nadat de kleur is toegekend aan SelectedColor, zowel bij klik op het kleurveld, als op de border van het kleurveld

        private void Items_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Rectangle rect && rect.Fill is SolidColorBrush solidColorBrush)
            {
                SelectedColor = solidColorBrush;
                _popup.IsOpen = false;
            }
            else if (e.OriginalSource is Border border && border.Background is SolidColorBrush borderSolidColorBrush2)
            {
                SelectedColor = borderSolidColorBrush2;
                _popup.IsOpen = false;
            }
            RaiseEvent(new RoutedEventArgs(SelectedColorChangedEvent));  // start het event met de uitvoering van (een) gebruikers-delegate(s)
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

            // Initialiseer de drie samenstellende onderdelen
            _rect = (Rectangle) GetTemplateChild(CP_Rectangle);
            _popup = (Popup)GetTemplateChild(CP_Popup);
            _items = (ItemsControl)GetTemplateChild(CP_Items);

            if (_rect != null)
            {
                // Initialiseer de rechthoek in de muis-eventhandler voor de rechthoek
                UpdateRectangle();
                _rect.Cursor = Cursors.Hand;
                _rect.MouseLeftButtonUp += Rectangle_MouseLeftButtonUp;
            }

            if (_items != null)
            {
                // Initializeer de items, en de eventhandlers van de items
                _items.ItemsSource = PredefinedBrushes;
                _items.MouseLeftButtonUp -= Items_MouseLeftButtonUp;
                _items.MouseLeftButtonUp += Items_MouseLeftButtonUp;
            }
        }
    }
}
