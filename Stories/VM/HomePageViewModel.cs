using Stories.Models;
using Stories.VM.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.VM
{
    public class HomePageViewModel
    {
        public List<string> Tags { get; set; }
        public List<Category> Categories { get; set; }
        public List<PostResponse> Posts { get; set; }
        public List<Post> LastPosts { get; set; }
    }
}
