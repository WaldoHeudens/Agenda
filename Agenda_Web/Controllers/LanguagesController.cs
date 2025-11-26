using Agenda_Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Agenda_Web.Controllers
{
    public class LanguagesController : Controller
    {

        private readonly AgendaDbContext _context;

        public LanguagesController(AgendaDbContext context)
        {
            _context = context;
        }

        public IActionResult ChangeLanguage(string code, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(code)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1) }
                );

            return LocalRedirect(returnUrl);
        }
    }
}
