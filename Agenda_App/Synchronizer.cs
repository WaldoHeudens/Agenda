using Agenda_App.Pages;
using Agenda_App.ViewModels;
using Agenda_Models;
using Agenda_Models.Resources;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using AppointmentType = Agenda_Models.AppointmentType;

namespace Agenda_App
{
    internal class Synchronizer
    {
        HttpClient client;
        JsonSerializerOptions sOptions;
        internal bool dbExists = false;
        
        readonly LocalDbContext _context;

        internal Synchronizer(LocalDbContext context)
        {
            _context = context;

            client = new HttpClient();
            client = new HttpClient();
            sOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,  // Tijdelijk voor betere leesbaarheid bij het debuggen
            };
        }


        async Task AllTypes()
        {
            // Synchroniseer eerst van lokaal naar API : Nog niet geïmplementeerd !
            foreach (AppointmentType type in _context.AppointmentTypes)
            {
                if (type.Id < 0)  // Aangepast of nieuw type
                {
                }
            }

            //  Synchroniseer nu van API naar lokaal
            if (await IsAuthorized())
            {
                Uri uri = new Uri(General.ApiUrl + "AppointmentTypes");
                try
                {
                    // Haal de afspraaktypes op via de API 
                    HttpResponseMessage response = await client.GetAsync(uri);

                    //response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    List<AppointmentType> types = JsonSerializer.Deserialize<List<AppointmentType>>(responseBody, sOptions);
                    if (types != null && types.Count > 0)
                    {
                        // Zet alle bestaande types op "deleted"
                        foreach (AppointmentType type in _context.AppointmentTypes)
                        {
                            type.Deleted = DateTime.Now;
                            _context.Update(type);
                        }
                        await _context.SaveChangesAsync();

                        // Pas nu de opgehaalde lijst toe
                        foreach (AppointmentType type in types)
                        {
                            AppointmentType existingType = null;
                            try
                            { 
                                // Bestaand type, dus bijwerken
                                existingType = _context.AppointmentTypes.Where(t => t.Id == type.Id).First();
                                existingType.Name = type.Name;
                                existingType.Description = type.Description;
                                existingType.Color = type.Color;
                                existingType.Deleted = type.Deleted;  // Niet verwijderd
                                _context.Update(existingType);
                            }
                            catch  // Nieuw type, dus toevoegen
                            {
                                try { _context.Add(type); }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception e) { }
            }
        }


        internal async Task<bool> IsAuthorized ()
        {
            // Als de autorisatie al bevestigd werd, return true
            if (General.UserId.Length>10)
                return true;

            // Vraag de API of de gebruiker geautoriseerd is
            Uri uri = new Uri(General.ApiUrl + "isauthorized");
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                //response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                General.UserId = JsonSerializer.Deserialize<string>(responseBody, sOptions);
                return General.UserId.Length > 10;  // Hebben we een userId teruggekregen of niet?
            }
            catch (Exception)
            {
                // Zet een waarde in UserId om te tonen dat we geprobeerd hebben om aan te melden
                General.UserId = "-";  
                return false;
            }
        }

        internal async Task InitializeDb()
        {
            // Verwijder eenmalig de database om problemen met migraties te vermijden
            //     Gebruik volgende regel als er geen "dirty" records verzonden moeten worden, maar er
            //     wel incompatibiliteitsproblemen met de locale database zijn.
            // await _context.Database.EnsureDeletedAsync();

            // Zorg ervoor dat de database is aangemaakt en de laatste migraties zijn toegepast
            _context.Database.MigrateAsync();


            // Zolang er nog geen synchronisatie is geweest, voeg een basis AppointmentType toe
            if (!_context.AppointmentTypes.Any())
            {
                Language nederlands = new Language { Code = "nl", Name = "Nederlands" };
                _context.Languages.Add(nederlands);
                AgendaUser user = new AgendaUser { UserName = "LocalUser", Email = "(local)", FirstName = "Local", LastName = "User", Language = nederlands };
                _context.Users.Add(user);
                _context.SaveChanges();
                _context.AppointmentTypes.Add(new Agenda_Models.LocalAppointmentType { Name = "?", User = user });
                _context.SaveChanges();
            }

            // Set de teller voor lokale ID's terug naar de laagste actuele waarde om 
            // ondubbelzinnige ID's te garanderen
            General.LocalIdCounter = (_context.Appointments.Any()) ?
                _context.Appointments.Min(a => a.Id) :
                1;
            long even = Math.Min( (_context.AppointmentTypes.Any()) ?
                                    _context.AppointmentTypes.Min(t => t.Id) : -1,
                                  -1);
            // Zet de teller onder de laagste waarde
            General.LocalIdCounter = Math.Min(General.LocalIdCounter, even ) - 1;

            dbExists = true;
        }

        internal async Task<bool> Login(LoginModel loginModel)
        {
            Uri uri = new Uri(General.ApiUrl + "login");

            try
            {
                // Verwijder oude logingegevens
                var oldLogins = _context.Logins.Where(l => l.Username == loginModel.Username);
                _context.Logins.RemoveRange(oldLogins);
                _context.SaveChanges();

                // Maak de content voor de (post) login-aanvraag
                string jsonString = JsonSerializer.Serialize(loginModel, sOptions);
                HttpContent content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                // Verstuur de login-aanvraag naar de API en verwerk de response
                HttpResponseMessage response = await client.PostAsync(uri, content);
                string responseBody = response.Content.ReadAsStringAsync().Result;
                // General.UserId = JsonSerializer.Deserialize<string>(responseBody, sOptions);
                General.UserId = responseBody.Trim('"');  // Verwijder aanhalingstekens uit de string
                
                if (General.UserId.Length > 10)  // Geldig UserId is teruggekomen
                {
                    loginModel.ValidTill = DateTime.Now + TimeSpan.FromHours(1);
                    _context.Add(loginModel);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                // Geen aanmelding, dus geen geldig UserId
                General.UserId = "-";
            }
            return false;
        }

        private async Task LoginToAPI()
        {
            // Controleer of de gebruiker al aangemeld is
            if (await IsAuthorized())
                return;

            // Haal recente logingegevens op om aan te melden
            try
            {
                LoginModel lm = _context.Logins.First(l => l.ValidTill > DateTime.Now
                                                            || l.RememberMe);
                if (lm != null)
                {
                    // Probeer aan te melden met de huidige credentials
                    if (await Login(lm))
                        return;
                }
            }
            catch(Exception e)  // Open het login-scherm als er geen geldige login is
            {
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage(new LoginViewModel(_context), _context));
            }
        }

        internal async Task SynchronizeAll()
        {
            // Synchroniseer lokale wijzigingen met de server en vice versa
            await LoginToAPI();

            // Als de loginprocedure goed is verlopen
            if (General.UserId.Length > 0)
            {
                await AllTypes();
            }
        }

    }
}
