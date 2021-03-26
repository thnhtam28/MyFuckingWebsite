using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stories.Services;
using Stories.VM;
using Stories.VM.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Stories.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBlogService _blogService;
        private IConfiguration _config;

        public UserController(IConfiguration config,
            IUserService userService,
            IBlogService blogService)
        {
            _config = config;
            _userService = userService;
            _blogService = blogService;
        }

        [HttpGet("/LogIn")]
        public IActionResult LogIn()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (!isAuthenticated)
                return View();
            return RedirectToAction("Index", "Blog");
        }

        [HttpPost]
        public async Task<IActionResult> Authentication(LogInRequest login)
        {
            var user = await _userService.AuthenticateUser(login);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Username", user.Username),
                    new Claim("Name", user.Name),
                    new Claim("IsAuthor", user.IsAuthor ? "true" : "false"),
                    new Claim(ClaimTypes.Role, user.IsAuthor ? "Admin" : "User")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(180)
                    });

                return RedirectToAction("Index", "Blog");
            }

            TempData["Error"] = "Sai tài khoản hoặc mật khẩu!";

            return RedirectToAction("LogIn", "User");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Blog");
        }

        [Route("/Register")]
        public IActionResult Register()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (!isAuthenticated)
                return View();
            return RedirectToAction("Index", "Blog");
        }

        [Route("/team")]
        public async Task<IActionResult> Team()
        {
            return View();
        }

        [Route("/team/member/{username}")]
        public async Task<IActionResult> TeamMember(string username)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [Route("/PageSetting")]
        public IActionResult PageSetting()
        {
            return View();
        }

        #region API
        [HttpPost]
        public async Task<JsonResult> GoogleOauth(string email)
        {
            var user = await _userService.GoogleAuthenticateUser(email);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Username", user.Username),
                    new Claim("Name", user.Name),
                    new Claim("IsAuthor", user.IsAuthor ? "true" : "false"),
                    new Claim(ClaimTypes.Role, user.IsAuthor ? "Admin" : "User")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(180)
                    });

                return Json(new
                {
                    status = 200,
                    message = "ok"
                });
            }

            return Json(new
            {
                status = 404,
                message = "Email chưa được đăng ký!"
            });
        }

        [HttpPost]
        public async Task<JsonResult> RegisterAccount(RegisterAccountRequest request)
        {
            var user = await _userService.RegisterAccount(request);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Username", user.Username),
                    new Claim("Name", user.Name),
                    new Claim("IsAuthor", user.IsAuthor ? "true" : "false"),
                    new Claim(ClaimTypes.Role, user.IsAuthor ? "Admin" : "User")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(180)
                    });

                return Json(new
                {
                    status = 200,
                    message = "ok"
                });
            }
            else if (user == null && request.UsingGoogleAuth)
            {
                user = await _userService.GoogleAuthenticateUser(request.Email);
                var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Username", user.Username),
                    new Claim("Name", user.Name),
                    new Claim("IsAuthor", user.IsAuthor ? "true" : "false"),
                    new Claim(ClaimTypes.Role, user.IsAuthor ? "Admin" : "User")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(180)
                    });

                return Json(new
                {
                    status = 200,
                    message = "ok"
                });
            }

            return Json(new { 
                status = 404,
                message = "Tên tài khoản hoặc email đã tồn tại!"
            });
        }
        #endregion
    }
}
