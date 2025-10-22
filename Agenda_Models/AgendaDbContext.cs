using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
            // Definieer de default connectiestring voor je project in je localhost
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=AgendaDb;Trusted_Connection=true;MultipleActiveResultSets=true";

            // Om de connectiestring te gebruiken die in je User Secrets moet je
            // - een ConnectionString toevoegen aan de User Secrets van dit Model-project (deze class library)
            // - de nodige PackageReferences toevoegen aan je projectfile ("Microsoft.Extensions.Configuration"-files:
            //      Zie de projectfile van dit project) voor de juiste .NET versie 
            // - moet je een geschikte configuratie bouwen met de ConfigurationBuilder().Build():
            if (!optionsBuilder.IsConfigured)
            {
                try
                {
                    var config = new ConfigurationBuilder()
                                        .SetBasePath(AppContext.BaseDirectory)  // De directory van de Model-Library, niet van het uitvoerend project
                                        .AddJsonFile("appsettings.json", optional: true) // Haal de connectionstring uit de Json-file
                                        .AddUserSecrets<AgendaDbContext>(optional: true) // Voeg de User Secrets toe
                                        .AddEnvironmentVariables()
                                        .Build();

                    string con = config.GetConnectionString("ServerConnection"); // ServerConnection is de naam die ik in mijn User Secrets aan de connectionstring heb gegeven
                    if (!con.IsNullOrEmpty())
                        connectionString = con;
                }
                catch (Exception ex) { }
            }

            optionsBuilder.UseSqlServer(connectionString);
        }

        public static void Seeder(AgendaDbContext context)
        {
            AgendaUser.Seeder();

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
