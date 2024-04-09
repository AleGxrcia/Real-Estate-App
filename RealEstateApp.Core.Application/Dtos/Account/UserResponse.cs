namespace RealEstateApp.Core.Application.Dtos.Account
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhotoUrl { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public string? IdNumber { get; set; }



        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
