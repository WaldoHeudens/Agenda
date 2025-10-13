using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda_Models
{
    public class AgendaDbContext: IdentityDbContext<AgendaUser>
    {
        public DbSet<AppointmentType> AppointmentTypes { get; set; }
        public DbSet<Appointment> Appointments { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string connectionString = "Server=localhost;Database=AgendaDb;User Id=sa;Password=Your_password123;MultipleActiveResultSets=true";
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=AgendaDb;Trusted_Connection=true;MultipleActiveResultSets=true";
            
            optionsBuilder.UseSqlServer(connectionString);
        }

        public static void Seeder(AgendaDbContext context)
        {
            if (!context.AppointmentTypes.Any())
            {
                context.AppointmentTypes.AddRange(AppointmentType.SeedingData());
                context.SaveChanges();
            }
            AppointmentType.Dummy = context.AppointmentTypes.First(at => at.Name == "-");

            if (!context.Appointments.Any())
            {
                context.Appointments.AddRange(Appointment.SeedingData());
                context.SaveChanges();
            }
        }
    }
}
