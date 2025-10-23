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
        [ForeignKey("AgendaUser")]
        public string UserId { get; set; } = AgendaUser.dummy.Id;
        public AgendaUser? User { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

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
}
