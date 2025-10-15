using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda_Models
{
    public class AgendaUser : IdentityUser
    {
        // Extra eigenschappen voor de gebruiker kunnen hier worden toegevoegd
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public static AgendaUser dummy = new AgendaUser
        {
            Id="-",
            FirstName = "-",
            LastName = "-",
            UserName = "Dummy",
            NormalizedUserName = "DUMMY",
            Email = "Dummy@Agenda.be",
            LockoutEnabled = true,
            LockoutEnd = DateTimeOffset.MaxValue
        };

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        public static async Task Seeder()
        {
            AgendaDbContext context = new AgendaDbContext();

            // Voeg de nodige rollen toe
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(new List<IdentityRole>
                {
                    new IdentityRole { Id = "Admin", Name = "Admin", NormalizedName = "ADMIN" },
                    new IdentityRole { Id= "SystemAdmin", Name = "SystemAdmin", NormalizedName = "SYSTEMADMIN" },
                    new IdentityRole { Id = "UserAdmin", Name = "UserAdmin", NormalizedName = "USERADMIN" },
                    new IdentityRole { Id = "User", Name = "User", NormalizedName = "USER" }
                });
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                context.Add(dummy);
                context.SaveChanges();
                AgendaUser user = new AgendaUser { UserName = "user", FirstName = "User", LastName = "Test", Email = "user.Test@Agenda.be", EmailConfirmed = true };
                AgendaUser admin = new AgendaUser { UserName = "admin", FirstName = "Admin", LastName = "Test", Email = "admin.Test@Agenda.be", EmailConfirmed = true };
                AgendaUser systemAdmin = new AgendaUser { UserName = "systemA", FirstName = "SystemA", LastName = "Test", Email = "systemA.Test@Agenda.be", EmailConfirmed = true };
                AgendaUser UserAdmin = new AgendaUser { UserName = "userA", FirstName = "UserA", LastName = "Test", Email = "userA.Test@Agenda.be", EmailConfirmed = true };
                UserManager<AgendaUser> userManager = new UserManager<AgendaUser>(
                    new UserStore<AgendaUser>(context),
                    null, new PasswordHasher<AgendaUser>(),
                    null, null, null, null, null, null);

                await userManager.CreateAsync(user, "Abc!12345");
                await userManager.CreateAsync(admin, "Abc!12345");
                await userManager.CreateAsync(systemAdmin, "Abc!12345");
                await userManager.CreateAsync(UserAdmin, "Abc!12345");

                while (context.Users.Count() < 5)
                {
                    await Task.Delay(100);
                }

                await userManager.AddToRoleAsync(user, "User");
                await userManager.AddToRoleAsync(admin, "Admin");
                await userManager.AddToRoleAsync(systemAdmin, "SystemAdmin");
                await userManager.AddToRoleAsync(UserAdmin, "UserAdmin");
            }

            dummy = context.Users.First(u => u.UserName == "Dummy");
        }
    }
}
