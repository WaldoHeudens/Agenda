﻿// See https://aka.ms/new-console-template for more information

using Agenda_Cons;
using Agenda_Models;

using (var context = new AgendaDbContext())
{
    AgendaDbContext.Seeder(context);

    // Haal alle appointments op en toon deze
    var alleAppointments = context.Appointments;
    Console.WriteLine("Alle afspraken: ");
    foreach (var appointment in alleAppointments)
    {
        Console.WriteLine("- " + appointment);
    }
    

    // Haal de appointments op die niet verwijderd zijn
    // Gebruik Where met een gedelegeerde functie
    var appointments = context.Appointments.Where(NotDeleted);
        bool NotDeleted(Appointment a)
        {
            return a.Deleted > DateTime.Now;
        }

    // Doe exact hetzelfde met een anonieme delegate
    appointments = context.Appointments
                          .Where(delegate (Appointment a){return a.Deleted > DateTime.Now;});

    // Doe weer exact hetzelfde met een lambda-expressie
    appointments = context.Appointments.Where(a => a.Deleted > DateTime.Now 
                                                && a.From > DateTime.Now
                                                && a.AppointmentType.Name != "Doctor")
                                        .OrderBy(a => a.Title);

    // Toon de gefilterde appointments
    Console.WriteLine("\nAlleen afspraken die niet verwijderd werden:");
    foreach (var appointment in appointments)
    {
        Console.WriteLine(appointment);
    }

    // Toevoegen van een afspraak
    Appointment nieuweAfspraak = new Appointment() { Title = "Tandardsbezoek", 
                                                     Description = "Dat gaat pijn doen",
                                                     From = DateTime.Now.AddDays(7)};
    context.Add(nieuweAfspraak);   // hetzelfde als context.Appointments.Add(nieuweAfspraak);
    context.SaveChanges();
    Console.WriteLine("\nEen afspraak werd toegevoegd:");
    foreach (var appointment in appointments)
    {
        Console.WriteLine(appointment);
    }

    // Wijzigen van een afspraak
    Appointment teWijzigen = appointments.FirstOrDefault(a => a.Title == "Tandardsbezoek");
    if (teWijzigen != null)
    {
        teWijzigen.From = DateTime.Now.AddDays(30);
        teWijzigen.Title = "Doktersbezoek";
        teWijzigen.Description = "Dat gaat minder pijn doen";
        teWijzigen.AppointmentType = context.AppointmentTypes.FirstOrDefault(at => at.Name == "Doctor");
        // teWijzigen.AppointmentTypeId = context.AppointmentTypes.FirstOrDefault(at => at.Name == "Doctor").Id;
        context.Update(teWijzigen);  // hetzelfde als context.Appointments.Update(teWijzigen);
        context.SaveChanges();

        Console.WriteLine("\nEen afspraak werd toegevoegd:");
        foreach (var appointment in appointments)
        {
            Console.Write(appointment);
            Console.Write("  (");
            Console.WriteLine(appointment.AppointmentType == null ? "-" : appointment.AppointmentType.Name + ")");
        }
    }



}


