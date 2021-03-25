using Stories.Models;
using Stories.VM.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.VM
{
    public class SearchResultViewModel
    {
        public bool Tag { get; set; }
        public string KeyWord { get; set; }
        public string Hashtag { get; set; }
        public int Total { get; set; }
        public List<Post> MostPopularPosts { get; set; }
        public List<PostResponse> SearchResultPosts { get; set; }
    }
}
