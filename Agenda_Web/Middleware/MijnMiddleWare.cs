using Agenda_Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Agenda_Web.Middleware
{
    public class MijnGebruiker
    {
        // Dit is een middleware voorbeeld dat een object van de huidige UserId ophaalt


        // Nodig om volgende stap in de requestafhandeling aan te roepen
        readonly RequestDelegate _next;

        Dictionary<string, string> Gebruikers;  // Cache van gebruikers

        AgendaDbContext _context;

        public MijnGebruiker(RequestDelegate next)
        {
            // Dit is mijn middleware constructor met de nodige code
            Gebruikers = new Dictionary<string, string>();
            Gebruikers["?"] = AgendaUser.dummy.Id;
            _context = new AgendaDbContext();

            // Als MijnGebruiker is afgehandeld, ga dan naar de volgende middleware in de pipeline
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // Dit is de code die wordt uitgevoerd voor elke HTTP-aanvraag
            string gebruikerNaam = httpContext.User.Identity?.Name;
            string userId = "";

            if (gebruikerNaam != null && !Gebruikers.ContainsKey(gebruikerNaam))
            {
                // Simuleer het ophalen van de gebruiker uit de database
                if (Gebruikers.ContainsKey(gebruikerNaam))
                    userId = Gebruikers[gebruikerNaam];
                if (userId == "")
                {
                    // Voeg de gebruiker toe aan de HttpContext-items voor later gebruik
                    userId = (await _context.Users.FirstAsync(u => u.UserName == gebruikerNaam)).Id;
                    Gebruikers[gebruikerNaam] = userId;
                }
            }
            else userId = AgendaUser.dummy.Id;
                httpContext.Items["UserId"] = userId;

            await _next(httpContext);
        }
    }
}