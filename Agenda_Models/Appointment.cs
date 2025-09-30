using System.ComponentModel.DataAnnotations;

namespace Agenda_Models
{
    public class Appointment
    {
        private DateTime now = DateTime.Now + new TimeSpan(0,1,0);
        private DateTime _from = DateTime.Now + new TimeSpan(1, 0, 0, 0);

        public long Id { get; set; } // = -1;  Niet nodig, wordt automatisch gegenereerd    

        [Required]
        [Display(Name = "Vanaf")]
        [DataType(DataType.DateTime)]
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
        [Display(Name = "Tot")]
        public DateTime To { get; set; } = DateTime.Now + new TimeSpan(1,1,30,0);

        [Required]
        [Display(Name = "Titel")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Omschrijving")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Hele dag")]
        public bool AllDay { get; set; } = false;

        [Required]
        [Display(Name = "Aangemaakt")]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Verwijderd")]
        public DateTime Deleted { get; set; } = DateTime.MaxValue;
    }
}
