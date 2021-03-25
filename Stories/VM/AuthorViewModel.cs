using Stories.Models;
using Stories.VM.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.VM
{
    public class AuthorViewModel
    {
        public User User { get; set; }
        public int Count { get; set; }
        public List<Post> MostPopularPosts { get; set; }
        public List<CommentResponse> LastComments { get; set; }
    }
}
