using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.VM.Response
{
    public class PostResponse
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CategoryId { get; set; }
        public string Category { get; set; }
        public string CategoryColor { get; set; }
        public string ImageLink { get; set; }
        public int ReadMinute { get; set; }
        public int Views { get; set; }
        public string Link { get; set; }
        public string Type { get; set; }
        public List<string> Tags { get; set; }
    }
}
