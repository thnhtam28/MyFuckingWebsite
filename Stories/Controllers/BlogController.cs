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

        [Route("blog")]
        public async Task<IActionResult> Index()
        {
            var posts = await _blogService.GetHomePagePosts(DateTime.Now.Year, 4);

            return View(posts);
        }

        [Route("blog/single")]
        public IActionResult SingleBlog()
        {
            return View();
        }

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
