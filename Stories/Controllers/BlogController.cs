using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stories.Services;
using System;
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
        [Route("blog/cat/{cat}")]
        [Route("blog/tag/{tag}")]
        [Route("blog/author/{username}")]
        public async Task<IActionResult> Index(string cat, string tag, string search_key, string username, int page = 1)
        {
            var posts = await _blogService.GetHomePagePosts(cat, tag, search_key, username, page);
            ViewBag.page = page;
            return View(posts);
        }

        [Route("blog/{link}")]
        public async Task<IActionResult> SingleBlog(string link)
        {
            var vm = await _blogService.GetSinglePost(link);
            return View(vm);
        }
    }
}
