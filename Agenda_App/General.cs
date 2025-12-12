using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda_App
{
    internal static class General
    {
        // De URL van de API
        internal static readonly string ApiUrl = "http://localhost:5128/api/";  // Werkt voor Windows machine

        // De URL van de DevTunnel:  Eén van deze twee regels activeren afhankelijk van het gebruikte platform
        //      Opmerking: De Url moet telkens na opstart van DevTunnel aangepast worden!
        //internal static readonly string ApiUrl = "https://960xxt9s-5128.euw.devtunnels.ms/api/"; // Werkt voor Android emulator met devtunnel

        // De UserId van de aangemelde gebruiker.  Wordt verkregen bij de aanmelding bij de API
        internal static string UserId = "";

        // De teller voor het veilig maken van een lokale ID voor nieuwe items (altijd negatief)
        internal static long LocalIdCounter = -1;
    }
}
