using Agenda_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Agenda_WPF
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly AgendaDbContext _context;
        private readonly UserManager<AgendaUser> _userManager;

        public RegisterWindow(AgendaDbContext context, UserManager<AgendaUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            InitializeComponent();
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (tbUsername.Text.IsNullOrEmpty())
            {
                tbError.Text = "Je moet een username invullen.";
                return;
            }
            if (tbFirstName.Text.IsNullOrEmpty())
            {
                tbError.Text = "Je moet een voornaam invullen.";
                return;
            }
            if (tbLastName.Text.IsNullOrEmpty())
            {
                tbError.Text = "Je moet een achternaa invullen.";
                return;
            }
            if (tbEmail.Text.IsNullOrEmpty())
            {
                tbError.Text = "Je moet een e-mailadres opgeven.";
                return;
            }
            if (pbPassword.Password.IsNullOrEmpty())
            {
                tbError.Text = "Je moet een wachtwoord invullen.";
                return;
            }
            if (pbConfirmPassword.Password.IsNullOrEmpty())
            {
                tbError.Text = "Je moet het wachtwoord bevestigen.";
                return;
            }
            if (pbPassword.Password != pbConfirmPassword.Password)
            {
                tbError.Text = "De wachtwoorden komen niet overeen.";
                return;
            }
            AgendaUser newUser = new AgendaUser
            {
                UserName = tbUsername.Text,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Email = tbEmail.Text,
                EmailConfirmed = true,
                LockoutEnabled = false,
                TwoFactorEnabled = false
            };
            var result = _userManager.CreateAsync(newUser, pbPassword.Password).Result;
            if (result.Succeeded)
            {
                MessageBox.Show("Je account is aangemaakt. Je kan nu inloggen.");
                _context.Add(new IdentityUserRole<string>() { RoleId = "User", UserId = newUser.Id });
                _context.SaveChanges();
                this.Close();
            }
            else
            {
                tbError.Text = string.Join("\n", result.Errors.Select(err => err.Description));
                return;
            }
        }
    }
}
