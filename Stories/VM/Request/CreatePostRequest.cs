using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.VM.Request
{
    public class CreatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public string CategoryId { get; set; }
        public string ImageLink { get; set; }
        public string Tag { get; set; }
        public string Type { get; set; }
        public bool Featured { get; set; }
    }
}
