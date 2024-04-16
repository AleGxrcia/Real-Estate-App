using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Dashboard;

namespace RealEstateApp.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPropertyService _propertyService;

        public AdminController(IUserService userService, IPropertyService propertyService)
        {
            _userService = userService;
            _propertyService = propertyService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsers();
            var properties = await _propertyService.GetAllViewModel();

            var dashboardVm = new AdminDashboardViewModel()
            {
                TotalActiveAgents = users.Where(u => u.IsActive == true && u.Role == Roles.Agent.ToString()).Count(),
                TotalInactiveAgents = users.Where(u => u.IsActive == false && u.Role == Roles.Agent.ToString()).Count(),
                TotalActiveAdmins = users.Where(u => u.IsActive == true && u.Role == Roles.Admin.ToString()).Count(),
                TotalInactiveAdmins = users.Where(u => u.IsActive == false && u.Role == Roles.Admin.ToString()).Count(),
                TotalActiveDevs = users.Where(u => u.IsActive == true && u.Role == Roles.Developer.ToString()).Count(),
                TotalInactiveDevs = users.Where(u => u.IsActive == false && u.Role == Roles.Developer.ToString()).Count(),
                TotalPropertiesRegistered = properties.Count()
            };

            return View(dashboardVm);
        }
    }
}
