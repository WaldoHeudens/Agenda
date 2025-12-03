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
        
        public AccountsController(SignInManager<AgendaUser> signInManager) 
        { 
            _signInManager = signInManager;
        }

        // POST: api/Accounts/LogIn
        [HttpPost]
        [Route("/api/Login")]
        public async Task<IActionResult> LogIn([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }

    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
