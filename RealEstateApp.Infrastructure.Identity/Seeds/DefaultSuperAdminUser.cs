using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Infrastructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultSuperAdminUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultUser = new();
            defaultUser.UserName = "superadminuser";
            defaultUser.Email = "superadminuser@email.com";
            defaultUser.FirstName = "John";
            defaultUser.LastName = "Doe";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;
            defaultUser.IsActive = true;
            defaultUser.Properties = 0;


            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var userEmail = await userManager.FindByEmailAsync(defaultUser.Email);
                var userName = await userManager.FindByNameAsync(defaultUser.UserName);

                if (userEmail == null && userName == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Client.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Agent.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Developer.ToString());

                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }
            }

        }
    }
}
