using Stories.Models;
using Stories.VM.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.VM
{
    public class BlogSingleViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorName { get; set; }

        public string AuthorAvatar { get; set; }

        public string AuthorDescription { get; set; }
        public int AuthorPostCount { get; set; }

        public string CategoryId { get; set; }

        public int ReadMinute { get; set; }

        public string ImageLink { get; set; }

        public string Tag { get; set; }

        public string Type { get; set; }

        public string Link { get; set; }

        public int Views { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public bool Status { get; set; }

        public List<CommentResponse> Comments { get; set; }

        public List<Post> MorePosts { get; set; }
        public List<PostResponse> RelatedPosts { get; set; }
    }
}
