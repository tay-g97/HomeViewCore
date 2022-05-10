using System;
using System.Collections.Generic;
using System.Text;
using HomeView.Models.Account;

namespace HomeView.Services
{
    public interface ITokenService
    {
        public string CreateToken(UserIdentity user);
    }
}
