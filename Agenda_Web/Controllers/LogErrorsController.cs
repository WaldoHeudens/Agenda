using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agenda_Models;
using Microsoft.AspNetCore.Authorization;

namespace Agenda_Web.Controllers
{
    [Authorize(Roles = "SystemAdmin")]
    public class LogErrorsController : Controller
    {
        private readonly AgendaDbContext _context;

        public LogErrorsController(AgendaDbContext context)
        {
            _context = context;
        }

        // GET: LogErrors
        public async Task<IActionResult> Index()
        {
            return View(await _context.LogErrors.ToListAsync());
        }

        // GET: LogErrors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logError = await _context.LogErrors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logError == null)
            {
                return NotFound();
            }

            return View(logError);
        }


        // GET: LogErrors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logError = await _context.LogErrors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (logError == null)
            {
                return NotFound();
            }

            return View(logError);
        }

        // POST: LogErrors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logError = await _context.LogErrors.FindAsync(id);
            if (logError != null)
            {
                _context.LogErrors.Remove(logError);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogErrorExists(int id)
        {
            return _context.LogErrors.Any(e => e.Id == id);
        }
    }
}
