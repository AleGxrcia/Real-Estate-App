using Microsoft.AspNetCore.Identity;


namespace RealEstateApp.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? IdNumber { get; set; }
        public int? Properties { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsActive { get; set; }


    }
}
