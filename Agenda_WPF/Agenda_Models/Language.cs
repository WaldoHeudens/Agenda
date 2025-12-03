using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda_Models
{
    public class Language
    {
        public static List<Language> Languages = new List<Language>();
        public static Language Dummy = null;

        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsSystemLanguage { get; set; }
        public DateTime IsActive { get; set; }

        public static void Seeder()
        {
            AgendaDbContext context = new AgendaDbContext();

            if (!context.Languages.Any())
            {
                context.Languages.AddRange(
                    new Language { Code = "- ", Name = "?", IsSystemLanguage = false, IsActive = DateTime.UtcNow },
                    new Language { Code = "en", Name = "English", IsSystemLanguage = true, IsActive = DateTime.UtcNow },
                    new Language { Code = "nl", Name = "Nederlands", IsSystemLanguage = true, IsActive = DateTime.UtcNow },
                    new Language { Code = "fr", Name = "français", IsSystemLanguage = true, IsActive = DateTime.UtcNow },
                    new Language { Code = "es", Name = "Español", IsSystemLanguage = false, IsActive = DateTime.MaxValue },
                    new Language { Code = "de", Name = "Deutch", IsSystemLanguage = false, IsActive = DateTime.MaxValue }
                    );
                context.SaveChanges();
            }
            Languages = context.Languages.Where(l => l.IsActive < DateTime.Now).OrderBy(l => l.Name).ToList();
            Dummy = Languages.FirstOrDefault(l => l.Code == "-");
            return;
        }
    }
}
