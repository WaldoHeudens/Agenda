using Agenda_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Agenda_Web.API_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogErrorController : ControllerBase
    {
        readonly SignInManager<AgendaUser> _signInManager; 
        readonly AgendaDbContext _context;
        
        public LogErrorController(SignInManager<AgendaUser> signInManager, AgendaDbContext context) 
        { 
            _signInManager = signInManager;
            _context = context;
        }


        // POST: api/LogIn
        [HttpPost]
        [Route("/api/LogError")]
        // POST: api/LogErrors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        public async Task<ActionResult> PostAppointmentType(string application, [FromBody] LogError error)
        {
            if (application != "Agenda_App")
                return NoContent();
            
            _context.LogErrors.Add(error);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
