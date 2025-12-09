using Agenda_Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda_App.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        LocalDbContext _context;

        public LoginViewModel(LocalDbContext context)
        {
            _context = context;
        }

        [ObservableProperty]
        LoginModel loginModel;

        [ObservableProperty]
        string userName;

        [ObservableProperty]
        string password;
        
        [ObservableProperty]
        bool rememberMe;

        [ObservableProperty]
        string message;

        bool isMessageVisible = false;

        [RelayCommand]
        async void Login()
        {
            LoginModel loginModel = new LoginModel()
            {
                Username = UserName,
                Password = Password,
                RememberMe = RememberMe
            };
            Synchronizer synchronizer = new Synchronizer(_context);
            bool result = await synchronizer.Login(loginModel);
            if (result)
            {
                Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                isMessageVisible = true;
                Message = "Foutieve login.  Probeer opnieuw of registreer je.";
            }
        }

    }
}
