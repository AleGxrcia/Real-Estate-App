using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.User;
using WebApp.RealStateApp.Middlewares;


namespace RedSocial.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _service;
        public AuthController(IUserService service)
        {
            _service = service;
        }

        //----------------------------------------------LogIn--------------------------------------------

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }
        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AuthenticationResponse uservm = await _service.LoginAsync(vm);
            if (uservm != null && uservm.HasError != true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", uservm);
                
                return RedirectToRoute(new { controller = 
                    uservm.Roles.FirstOrDefault() == Roles.Agent.ToString() ? "Property" : 
                    uservm.Roles.FirstOrDefault() == Roles.Client.ToString()? "Client" : "Admin"
                    , action = "Index" });
            }
            else
            {
                vm.HasError = uservm.HasError;
                vm.Error = uservm.Error;
                return View(vm);
            }
        }


        //----------------------------------------------LogOut--------------------------------------------
        public async Task<IActionResult> LogOut()
        {
            await _service.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "Auth", action = "Index" });
        }

        //--------------------------------------------Register------------------------------------------------
        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Register()
        {
            return View(new SaveUserViewModel());
        }


        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];


            RegisterResponse response = await _service.RegisterAsync(vm, origin, vm.Photo);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }


            return RedirectToRoute(new { controller = "Auth", action = "Index" });

        }

        //-----------------------------------------------------ConfitmEmail------------------------------------
        public async Task<IActionResult> ConfirmEmail(string UserId, string token)
        {
            string response = await _service.ConfirmEmailAsync(UserId, token);
            return View("ConfirmEmail", response);
        }

        //---------------------------------------------------ForgotPassword-------------------------------
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];
            ForgotPasswordResponse response = await _service.ForgotPasswordAsync(vm, origin);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }


        //-----------------------------------------------------ResetPassword--------------------------------------
        public IActionResult ResetPassword(string token)
        {
            return View(new ResetPasswordViewModel { Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            ResetPasswordResponse response = await _service.ResetPasswordAsync(vm);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        //-------------------------------------------------Acces Denied-----------------------------------------------
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
