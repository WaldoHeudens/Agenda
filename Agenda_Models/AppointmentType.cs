using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda_Models
{
    public class AppointmentType
    {
        // Dummy instantie voor referentie
        static public AppointmentType Dummy = null;

        // [Key]
        public int Id { get; set; }

        [Required]
        [Display (Name = "Gebruiker")]
        [ForeignKey("AgendaUser")]
        public string UserId { get; set; } = AgendaUser.dummy.Id;
       
        [Display(Name = "Gebruiker")]
        public AgendaUser? User { get; set; }

        [Display(Name = "Type", ResourceType = typeof(Resources.AppointmentType))]
        [Required]
        public string Name { get; set; } = "";

        [Required]
        [Display(Name = "Description", ResourceType = typeof(Resources.AppointmentType))]
        public string Description { get; set; } = "";

        [Display(Name = "Color", ResourceType = typeof(Resources.AppointmentType))]
        public string Color { get; set; } = "#FF000000"; // Kleur waarin de afspraken getoond zullen worden Default zwart

        [Required]
        public DateTime Deleted { get; set; } = DateTime.MaxValue;


        public override string ToString()
        {
            return Name;
        }

        // Voorzie ook een seeding data
        public static List<AppointmentType> SeedingData()
        {
            var list = new List<AppointmentType>();
            list.AddRange(list = new List<AppointmentType>
            {
                // Voeg een dummy AppointmentType toe
                new AppointmentType { Name = "-", Description = "-", Deleted = DateTime.MaxValue },

                // Voeg enkele voorbeeld AppointmentTypes toe, met seeding van kleur
                new AppointmentType { Name = "Meeting", Description = "Business meeting", Color="#FF0000FF"},
                new AppointmentType { Name = "Doctor", Description = "Doctor's appointment", Color="#FFFF0000"},
                new AppointmentType { Name = "Holiday", Description = "Holiday event",  Color="#FF00FF00" },
                new AppointmentType { Name = "Conference", Description = "Professional conference", Color = "#FFFFA500" }
            });
            return list;
        }
    }

    public class  LocalAppointmentType:AppointmentType
    {
        // Deze klasse is nodig om lokale opslag te synchroniseren 

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public new int Id { get; set; }     // Definieer een nieuwe Id die niet door de databank wordt toegekend

    }
}
