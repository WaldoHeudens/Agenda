using Agenda_Models;
using Agenda_Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index(string application="", string level="", int? pageNumber = null)
        {
            var logList = _context.LogErrors
                        .Where (le => (le.Application.Contains(application)||application=="")
                                    && (level == "" || le.LogLevel.Contains(level)))
                        .OrderBy(le => le.TimeStamp);
            if (pageNumber == null) {pageNumber = 1;}

            PageList<LogError> model = await PageList<LogError>.CreateAsync(logList, pageNumber.Value, 10);

            ViewData["level"] = level;
            ViewData["application"] = application;
            return View(model);
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
