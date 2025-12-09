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
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly AgendaDbContext _context;

        public AppointmentsController(AgendaDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var agendaDbContext = _context.Appointments
                .Where(a => a.Deleted >= DateTime.Now
                            //&& a.UserId == _context.Users.First(u => u.UserName == User.Identity.Name).Id
                            // Gebruik de user toegevoegd in onze middleware
                            && a.UserId == (Request.HttpContext.Items["UserId"])
                            && a.From >= DateTime.Now)
                .OrderBy(a => a.From)
                .Include(a => a.AppointmentType);
            return View(await agendaDbContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.AppointmentType)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            //string userId = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            string userId = (string)Request.HttpContext.Items["UserId"];
            var appointmentTypes = _context.AppointmentTypes.Where(at => at.Deleted > DateTime.Now && at.UserId == userId);
            ViewData["AppointmentTypeId"] = new SelectList(appointmentTypes, "Id", "Name");
            Appointment model = new Appointment { UserId = userId };
            return View(model);
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,From,To,Title,Description,AllDay,Created,Deleted,AppointmentTypeId")] Appointment appointment)
        {
            //string userId = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            string userId = (string)Request.HttpContext.Items["UserId"];
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var appointmentTypes = _context.AppointmentTypes.Where(at => at.Deleted > DateTime.Now && at.UserId == userId);
            ViewData["AppointmentTypeId"] = new SelectList(appointmentTypes, "Id", "Name");
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            //string userId = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            string userId = (string)Request.HttpContext.Items["UserId"];
            var appointmentTypes = _context.AppointmentTypes.Where(at => at.Deleted > DateTime.Now && at.UserId == userId);
            ViewData["AppointmentTypeId"] = new SelectList(appointmentTypes, "Id", "Name");
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,UserId,From,To,Title,Description,AllDay,Created,Deleted,AppointmentTypeId")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }
            //string userId = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            string userId = (string)Request.HttpContext.Items["UserId"];

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
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
            var appointmentTypes = _context.AppointmentTypes.Where(at => at.Deleted > DateTime.Now && at.UserId == userId);
            ViewData["AppointmentTypeId"] = new SelectList(appointmentTypes, "Id", "Name");
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.AppointmentType)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Deleted = DateTime.Now;
                _context.Appointments.Update(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(long id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}
