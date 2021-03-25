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
        public List<PostResponse> FeaturedPosts { get; set; }
        public List<Post> MostPopularPosts { get; set; }
        public List<PostResponse> RandomCategoryPosts { get; set; }
        public List<string> HotTags { get; set; }
        public List<CommentResponse> LastComments { get; set; }
    }
}
