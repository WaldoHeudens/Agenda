using Agenda_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda_Models
{
    public class LocalDbContext : DbContext
    {
        public DbSet<Language> Languages { get; set; }

        public DbSet<LocalAppointmentType> AppointmentTypes { get; set; }

        public DbSet<LocalAppointment> Appointments { get; set; }

        public DbSet<AgendaUser> Users { get; set; }
        public DbSet<LoginModel> Logins { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var DbPath = System.IO.Path.Join(path, "AgendaApp.db");
            options.UseSqlite($"Data Source={DbPath}");

        }
    }
}
