using MicroManagement.Application.Common;
using MicroManagement.Application.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace My_Micro_Management.Features.Auth
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IAuthenticationService _authenticationService = MauiProgram.GetService<IAuthenticationService>();
        private readonly IAuthenticationContextProvider _authenticationContextProvider = MauiProgram.GetService<IAuthenticationContextProvider>();

        private string _userMail;
        public string UserMail
        {
            get { return _userMail; }
            set { _userMail = value; }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public async Task Login()
        {
            var (accessToken, refreshToken) = await this._authenticationService.Login(this.UserMail, this.Password);
            await this._authenticationContextProvider.Login(accessToken, refreshToken);
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
