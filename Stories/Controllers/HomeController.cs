using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Stories.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }        
        
        [Route("project")]
        public IActionResult Project()
        {
            return View();
        }

        [Route("project/single")]
        public IActionResult SingleProject()
        {
            return View();
        }

        [Route("showcase")]
        public IActionResult Showcase()
        {
            return View();
        }

        [Route("showcase/single")]
        public IActionResult SingleShowcase()
        {
            return View();
        }

        [Route("music")]
        public IActionResult Music()
        {
            return View();
        }

        [Route("music/single")]
        public IActionResult SingleMusic()
        {
            return View();
        }

        [Route("service")]
        public IActionResult Service()
        {
            return View();
        }

        [Route("coming")]
        public IActionResult ComingSoon()
        {
            ViewBag.d = new DateTime(2021, 9, 12).ToString("MM/dd/yyyy");
            return View();
        }

        [HttpPost]
        public IActionResult CultureManagement(string culture, string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30)});

            return LocalRedirect(returnUrl);
        }

        #region Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/Error/{code}")]
        public IActionResult Error(int code)
        {
            ViewBag.code = code;
            var message = "";

            if (code == 404)
            {
                message = "Đường dẫn bạn đang yêu cầu đã bị xóa hoặc thay đổi";
            }
            else if (code == 500)
            {
                message = "Server hoặc code bị lỗi gì đấy @@";
            }
            else
            {
                return Redirect("/Error/404");
            }

            ViewBag.message = message;
            return View();
        }
        #endregion
    }
}
