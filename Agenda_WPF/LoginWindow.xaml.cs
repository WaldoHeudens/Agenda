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

namespace Agenda_WPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly UserManager<AgendaUser> _userManager;

        public LoginWindow(UserManager<AgendaUser> userManager)
        {
            _userManager = userManager;
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!tbPassword.Password.IsNullOrEmpty() && !tbUsername.Text.IsNullOrEmpty())
            {
                AgendaUser? user = await _userManager.FindByNameAsync(tbUsername.Text);
                if (user != null)
                {
                    bool succeeded = await _userManager.CheckPasswordAsync(user, tbPassword.Password);
                    if (succeeded)
                    {
                        {
                            App.User = user;
                            App.MainWindow.mnNoUser.Visibility = Visibility.Collapsed;
                            App.MainWindow.mnUserKnow.Visibility = Visibility.Visible;
                            App.MainWindow.mniName.Header = user.ToString();
                            Close();
                        }
                    }
                    tbError.Text = "Ongeldige username of wachtwoord.";
                }
                else
                {
                    tbError.Text = "Je moet een username en wachtwoord invullen.";

                }
            }
        }
    }
}
