using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Dtos.Account
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string Identification { get; set; }
        public string Phone { get; set; }

        // Hacer el foto url
        public IFormFile? Photo { get; set; }


        public string? UserType { get; set; }

    }
}
