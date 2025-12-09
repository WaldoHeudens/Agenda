using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda_Models
{
    public class LoginModel
    {
        [Required]
        [Key]
        public string Username { get; set; }

        [PasswordPropertyText]
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
        public DateTime ValidTill { get; set; } = DateTime.Now + TimeSpan.FromHours(1);
    }
}
