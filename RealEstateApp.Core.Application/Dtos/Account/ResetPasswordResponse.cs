﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.Dtos.Account
{
    public class ResetPasswordResponse
    {
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
