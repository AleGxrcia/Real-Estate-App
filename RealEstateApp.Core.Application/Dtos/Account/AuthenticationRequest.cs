using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Dtos.Account
{
    public class AuthenticationRequest
    {
        public string EmailOrUserName { get; set; }
        public string Password { get; set; }
    }
}
