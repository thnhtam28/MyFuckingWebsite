using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stories.Models;
using Stories.Services;
using Stories.VM.Request;
using Stories.VM.Response;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;
        private readonly IBlogService _blogService;

        public BlogController(ILogger<BlogController> logger,
            IBlogService blogService)
        {
            _logger = logger;
            _blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _blogService.GetHomePagePosts(DateTime.Now.Year, 4);

            return View(posts);
        }

        [Authorize(Roles = "Admin")]
        [Route("/CreatePost")]
        public IActionResult CreatePost()
        {
            return View();
        }

        public async Task<IActionResult> Category(string id)
        {
            var category = await _blogService.GetCategory(id);
            var Count = await _blogService.CountPost(2, id);

            ViewBag.Count = Count;
            return View(category);
        }

        [Route("/{controller}/Single/{link}")]
        public async Task<IActionResult> Single(string link)
        {
            var post = await _blogService.GetPost(link);
            return View(post);
        }

        [HttpGet]
        [Route("/{controller}/Search")]
        [Route("/{controller}/Tag/{tag}")]
        public async Task<IActionResult> Search(string keyword, string tag)
        {
            var model = await _blogService.GetSearchResultPosts(keyword, tag, DateTime.Now.Year, 4);
            return View(model);
        }

        [Route("/About")]
        public IActionResult About()
        {
            return View();
        }

        [Route("/Contact")]
        public IActionResult Contact()
        {
            return View();
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

        #region API
        [HttpPost("/{controller}/CreatePost")]
        public async Task<JsonResult> CreatePost(CreatePostRequest request)
        {
            var post = await _blogService.CreatePost(request);
            return Json(post);
        }

        [HttpGet("/{controller}/GetLayoutResponse")]
        public async Task<JsonResult> GetLayoutResponse()
        {
            var layoutResponse = await _blogService.GetLayoutResponse();
            layoutResponse.Ad = new Ad {
                Link = "#",
                ImageLink = "https://kxge.somee.com/imgs/ads/ads-1.jpg",
                SponsoredBy = "bạn Nam giấu tên"
            };
            return Json(layoutResponse);
        }

        [HttpGet("/{controller}/GetCategories")]
        public async Task<JsonResult> GetCategories()
        {
            var post = await _blogService.GetCategories();
            return Json(post);
        }

        [HttpGet("/{controller}/GetLatestPosts")]
        public async Task<JsonResult> GetLatestPosts(int pageNumber)
        {
            var post = await _blogService.GetLatestPosts(pageNumber);
            return Json(post);
        }

        [HttpGet("/{controller}/GetAuthorPosts")]
        public async Task<JsonResult> GetPostByAuthor(string username, int pageNumber)
        {
            var post = await _blogService.GetPostByAuthor(username, pageNumber);
            return Json(post);
        }

        [HttpGet("/{controller}/GetPostByCategory")]
        public async Task<JsonResult> GetPostByCategory(string categoryId, int pageNumber)
        {
            var post = await _blogService.GetPostByCategory(categoryId, pageNumber);
            return Json(post);
        }

        [HttpGet("/{controller}/GetSearchResultPosts")]
        public async Task<JsonResult> GetSearchResultPosts(string keyword, int pageNumber)
        {
            var post = await _blogService.GetSearchResultPosts(keyword, pageNumber);
            return Json(post);
        }

        [HttpPost("/Comment/CreateComment")]
        public async Task<JsonResult> CreateComment(CreateCommentRequest request)
        {
            var success = false;

            var comment = await _blogService.CreateComment(request);

            if (comment != null) success = true;

            return Json(new
            {
                success = success,
                comment = comment
            });
        }
        #endregion
    }
}
