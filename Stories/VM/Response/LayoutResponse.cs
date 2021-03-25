using Stories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.VM.Response
{
    public class LayoutResponse
    {
        public List<Category> Categories { get; set; }
        public List<Category> SuggestedCategories { get; set; }
        public List<HotTopic> HotTopics { get; set; }
        public List<Post> DontMiss { get; set; }
        public Ad Ad { get; set; }
        public List<FooterPost> FooterPosts { get; set; }
        public List<string> TagCloud { get; set; }
    }

    public class HotTopic
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int PostCount { get; set; }
    }

    public class Ad
    {
        public string ImageLink { get; set; }
        public string Link { get; set; }
        public string SponsoredBy { get; set; }
    }

    public class FooterPost
    {
        public string CategoryName { get; set; }
        public List<Post> Posts { get; set; }
    }
}
