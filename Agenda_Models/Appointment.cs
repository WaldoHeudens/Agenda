using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agenda_Models
{
    public class Appointment
    {
        private DateTime now = DateTime.Now + new TimeSpan(0,1,0);
        private DateTime _from = DateTime.Now + new TimeSpan(1, 0, 0, 0);

        public long Id { get; set; } // = -1;  Niet nodig, wordt automatisch gegenereerd    
        [Required]
        [ForeignKey ("AgendaUser")]
        public string UserId { get; set; } = AgendaUser.dummy!= null ? AgendaUser.dummy.Id : "-";
        public AgendaUser? User { get; set; }

        [Required]
        [Display(Name = "Vanaf", ResourceType = typeof(Resources.Appointment))]
        [DataType(DataType.DateTime)]
        [DisplayFormat (DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime From
        {
            get => _from;
            set {if (value < now)
                    _from = now;
                else
                    _from = value;
            }
        }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tot", ResourceType = typeof(Resources.Appointment))]
        public DateTime To { get; set; } = DateTime.Now + new TimeSpan(1,1,30,0);

        [Required]
        [Display(Name = "Titel", ResourceType = typeof(Resources.Appointment))]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Omschrijving", ResourceType = typeof(Resources.Appointment))]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Hele_dag", ResourceType = typeof(Resources.Appointment))]
        public bool AllDay { get; set; } = false;

        [Required]
        [Display(Name = "Aangemaakt", ResourceType = typeof(Resources.Appointment))]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Verwijderd", ResourceType = typeof(Resources.Appointment))]
        public DateTime Deleted { get; set; } = DateTime.MaxValue;

        // Foreign key naar AppointmentType:  Zorg voor de juiste één-op-veel relatie
        [Required]
        [Display(Name = "Type", ResourceType = typeof(Resources.Appointment))]
        [ForeignKey("AppointmentType")]
        public int AppointmentTypeId { get; set; } = AppointmentType.Dummy!=null ? AppointmentType.Dummy.Id : 1; // Standaard naar de Dummy verwijzen

        // Navigatie-eigenschap
        [Display(Name = "Type", ResourceType = typeof(Resources.Appointment))]
        public AppointmentType? AppointmentType { get; set; }



        public override string ToString()
        {
            return Id + "  Afspraak op " + From + " betreffende " + Title;
        }


        public static List<Appointment> SeedingData()
        {
            var list = new List<Appointment>();
            list.AddRange(
                // Voeg een default-appointment toe
                new Appointment { Title = "-", Deleted = DateTime.Now },

                // Voeg enkele test-appointments toe
                new Appointment { Title = "Afspraak met Bob", From = DateTime.Now.AddDays(7), To = DateTime.Now.AddDays(7).AddHours(1) },
                new Appointment { Title = "Lunch met Alice", From = DateTime.Now.AddDays(12), To = DateTime.Now.AddDays(12).AddHours(2) },
                new Appointment { Title = "Projectbespreking", From = DateTime.Now.AddDays(15), To = DateTime.Now.AddDays(15).AddHours(3) }
                );

            return list;
        }
    }


    // Deze klasse is nodig om op een local device een eigen primary key te kunnen definiëren,
    // en te werken met LacalAppointmentTypes

    public class LocalAppointment : Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Geen automatische generatie van de primary key
        public new long Id { get; set; } // Override de Id om een eigen primary key te kunnen definiëren

        [ForeignKey("AppointmentType")]
        public new int AppointmentTypeId { get; set; } // Override de AppointmentTypeId om te werken met LocalAppointmentType
        public new LocalAppointmentType? AppointmentType { get; set; } // Override de AppointmentType om te werken met LocalAppointmentType
    }
}
