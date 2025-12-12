using Agenda_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Agenda_Web.API_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        readonly SignInManager<AgendaUser> _signInManager; 
        readonly AgendaDbContext _context;
        
        public AccountsController(SignInManager<AgendaUser> signInManager, AgendaDbContext context) 
        { 
            _signInManager = signInManager;
            _context = context;
        }

        // Post: api/IsAuthorized
        [HttpGet]
        [Route("/api/isauthorized")]
        public async Task<AgendaUser> IsAuthorized()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                // Als aangemeld, zend de user-Id terug
                return _context.Users.First(u => u.UserName == User.Identity.Name);
            }
            else
            {
                return AgendaUser.dummy;
            }
        }


        // POST: api/LogIn
        [HttpPost]
        [Route("/api/Login")]
        public async Task<AgendaUser> LogIn([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // Als aangemeld, zend de user-Id terug
                return _context.Users.First(u => u.UserName == User.Identity.Name);
            }
            else
            {
                return AgendaUser.dummy;
            }
        }
    }
}
