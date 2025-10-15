using Agenda_Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Agenda_WPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RolesWindow : Window
    {
        private readonly AgendaDbContext _context;
        private readonly UserManager<AgendaUser> _userManager;

        AgendaUser user;

        public RolesWindow(AgendaDbContext context, UserManager<AgendaUser> userManager)
        {
            _context= context;
            _userManager= userManager;

            InitializeComponent();

            cbUsers.ItemsSource = _context.Users
                                    .Where(u => u.LockoutEnd == null || u.LockoutEnd < DateTime.Now)
                                    .OrderBy(u => u.LastName + " " + u.FirstName)
                                    .ToList();
        }

        private void cbUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            user=_context.Users.Find(((AgendaUser)cbUsers.SelectedItem).Id);
            List<ListBoxItem> roles = new List<ListBoxItem>();
            List<string> userRoles = (from ur in _context.UserRoles
                                     where ur.UserId == user.Id
                                     select ur.RoleId)
                                     .ToList();
            foreach (IdentityRole role in _context.Roles)
            {
                bool isSelected = userRoles.Contains(role.Id);
                roles.Add(new ListBoxItem { Content = role.Id, IsSelected = isSelected });
            }
            lbRoles.ItemsSource = roles;
            lbRoles.Visibility = Visibility.Visible;
        }

        private async void lbRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(ListBoxItem item in lbRoles.Items)
            {
                string role = item.Content.ToString();
                if (lbRoles.SelectedItems.Contains(item))
                    await _userManager.AddToRoleAsync(user, role);
                else
                    await _userManager.RemoveFromRoleAsync(user, role);
            }
        }
    }
}
