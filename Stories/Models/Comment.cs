using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Models
{
    [Table("Comment", Schema = "public")]
    public class Comment
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("post_id")]
        public Guid PostId { get; set; }
        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("website")]
        public string Website { get; set; }
        [Column("id_reference")]
        public Guid? IdReference { get; set; }
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        public Comment()
        {
            Id = new Guid();
            CreatedDate = DateTime.Now;
        }
    }
}
