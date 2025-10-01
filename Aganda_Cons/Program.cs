// See https://aka.ms/new-console-template for more information

using Agenda_Cons;

using (var context = new AgendaDbContext())
{
    AgendaDbContext.Seeder(context);

    var appointments = context.Appointments.ToList();
    foreach (var appointment in appointments)
    {
        Console.WriteLine(appointment);
    }
}


