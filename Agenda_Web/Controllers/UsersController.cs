using Agenda_Cons.Migrations;
using Agenda_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Agenda_Web.Controllers
{
    [Authorize (Roles = "UserAdmin")]
    public class UsersController : Controller
    {
        private readonly AgendaDbContext _context;

        public UsersController(AgendaDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index(string username = "", string role = "?")
        {
            var agendaDbContext = from AgendaUser user in _context.Users
                                  where (user.UserName != "Dummy"
                                        && (username == "" || user.UserName.Contains(username)))
                                        && (role == "?" || (from ur in _context.UserRoles
                                                            where ur.UserId == user.Id
                                                            select ur.RoleId).Contains(role))
                                  orderby user.UserName
                                  select new UserViewModel
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      Email = user.Email,
                                      Blocked = user.LockoutEnd.HasValue ? user.LockoutEnd.Value.DateTime >= DateTime.MinValue: false,
                                      Roles = (from ur in _context.UserRoles
                                               where ur.UserId == user.Id
                                               select ur.RoleId).ToList()
                                  };

            ViewData["username"] = username;
            var roles = await _context.Roles.ToListAsync();
            roles.Add(new IdentityRole { Id = "?", Name = "?"});
            ViewData["Roles"] = new SelectList(roles, "Id", "Id", roles.First(r => r.Id == role));
            return View(await agendaDbContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> BlockUnblock(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value.DateTime >= DateTime.MinValue)
            {
                // Unblock user
                user.LockoutEnd = DateTimeOffset.MinValue;
            }
            else
            {
                // Block user
                user.LockoutEnd = DateTimeOffset.MaxValue;
            }
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Roles(string? Id)
        {
            if (Id == null)
                return RedirectToAction(nameof(Index));
            AgendaUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id==Id);
            UserRolesViewModel roleViewModel = new UserRolesViewModel
            {
                UserName = user.UserName,
                Roles = await (from userRole in _context.UserRoles
                         where userRole.UserId == user.Id
                         orderby userRole.RoleId
                         select userRole.RoleId).ToListAsync()
            };
            ViewData["AllRoles"] = new MultiSelectList(_context.Roles.OrderBy(r => r.Name), "Id", "Id", roleViewModel.Roles);
            return View(roleViewModel);
        }


        [HttpPost]
        public IActionResult Roles([Bind("UserName, Roles")] UserRolesViewModel _model)
        {
            AgendaUser user = _context.Users.FirstOrDefault(u => u.UserName == _model.UserName);

            // Bestaande rollen ophalen
            List<IdentityUserRole<string>> roles = _context.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
            foreach (IdentityUserRole<string> role in roles)
                _context.Remove(role);

            // Nieuwe rollen toekennen
            if (_model.Roles != null)
                foreach (string roleId in _model.Roles)
                    _context.UserRoles.Add(new IdentityUserRole<string> { RoleId = roleId, UserId = user.Id });

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }

    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Gebruiker")]
        public string UserName { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Blokkeren of deblokkeren")]
        public bool Blocked { get; set; }
       
        [Display(Name = "Rollen")]
        public List<string> Roles { get; set; }
    }

    public class UserRolesViewModel
    {
        [Display(Name = "User")]
        public string UserName { get; set; }

        [Display(Name = "Roles")]
        public List<string> Roles { get; set; }
    }

}
