using Agenda_Cons.Migrations;
using Agenda_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda_Web.Controllers
{
    [Authorize (Roles = "User")]
    public class AppointmentTypesController : Controller
    {
        private readonly AgendaDbContext _context;

        public AppointmentTypesController(AgendaDbContext context)
        {
            _context = context;
        }

        // GET: AppointmentTypes
        public async Task<IActionResult> Index()
        {
            try
            {
                string userId = _context.Users.First(u => u.UserName == User.Identity.Name).Id;

                var agendaDbContext = _context.AppointmentTypes
                                        .Where(a => a.Deleted > DateTime.Now
                                                        && a.UserId == userId);
                                        //.Include(a => a.User);
                return View(await agendaDbContext.ToListAsync());
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                return View(new List<AppointmentType>());
            }
        }

        // GET: AppointmentTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentType = await _context.AppointmentTypes
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentType == null)
            {
                return NotFound();
            }

            return View(appointmentType);
        }

        // GET: AppointmentTypes/Create
        public IActionResult Create()
        {
            // Niet meer nodig, want UserId wordt automatisch ingevuld
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");

            // We sturen wel een voorgedefinieerd model mee
            string userId = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            AppointmentType at = new AppointmentType { UserId = userId };
            return View(at);
        }

        // POST: AppointmentTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Name,Description,Color,Deleted")] AppointmentType appointmentType)
        {

            if (ModelState.IsValid)
            {
                _context.Add(appointmentType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Niet meer nodig, want UserId wordt automatisch ingevuld
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", appointmentType.UserId);
            return View(appointmentType);
        }

        // GET: AppointmentTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentType = await _context.AppointmentTypes.FindAsync(id);
            if (appointmentType == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", appointmentType.UserId);
            return View(appointmentType);
        }

        // POST: AppointmentTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Name,Description,Color,Deleted")] AppointmentType appointmentType)
        {
            if (id != appointmentType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentTypeExists(appointmentType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", appointmentType.UserId);
            return View(appointmentType);
        }

        // GET: AppointmentTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentType = await _context.AppointmentTypes
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentType == null)
            {
                return NotFound();
            }

            return View(appointmentType);
        }

        // POST: AppointmentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentType = await _context.AppointmentTypes.FindAsync(id);
            if (appointmentType != null)
            {
                _context.AppointmentTypes.Remove(appointmentType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentTypeExists(int id)
        {
            return _context.AppointmentTypes.Any(e => e.Id == id);
        }
    }
}
