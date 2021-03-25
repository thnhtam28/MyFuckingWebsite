using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.VM.Request
{
    public class CreateCommentRequest
    {
        public bool IsAuth { get; set; }
        public Guid? UserId { get; set; }
        public Guid PostID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Content { get; set; }
    }
}
