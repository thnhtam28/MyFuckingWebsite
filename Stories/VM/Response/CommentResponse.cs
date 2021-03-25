using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.VM.Response
{
    public class CommentResponse
    {
        public Guid Id { get; set; }
        public Guid? IdReference { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }

        public string Username { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }

        public string PostLink { get; set; }
    }
}
