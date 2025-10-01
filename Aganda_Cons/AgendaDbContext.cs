using Agenda_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda_Cons
{
    internal class AgendaDbContext: DbContext
    {
        public DbSet<Appointment> Appointments { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string connectionString = "Server=localhost;Database=AgendaDb;User Id=sa;Password=Your_password123;MultipleActiveResultSets=true";
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=AgendaDb;Trusted_Connection=true;MultipleActiveResultSets=true";
            
            optionsBuilder.UseSqlServer(connectionString);
        }

        internal static void Seeder(AgendaDbContext context)
        {
            if (!context.Appointments.Any())
            {
                context.Appointments.AddRange(Appointment.SeedingData());
                context.SaveChanges();
            }
        }
    }
}
