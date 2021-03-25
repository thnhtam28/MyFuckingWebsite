using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Models
{
    [Table("Post", Schema = "public")]
    public class Post
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("author_id")]
        public Guid AuthorId { get; set; }

        [Column("category_id")]
        public string CategoryId { get; set; }

        [Column("read_minute")]
        public int ReadMinute { get; set; }

        [Column("image")]
        public string ImageLink { get; set; }

        [Column("tag")]
        public string Tag { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("link")]
        public string Link { get; set; }

        [Column("views")]
        public int Views { get; set; }

        [Column("status")]
        public bool Status { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; }

        [Column("featured")]
        public bool Featured { get; set; }

        public Post()
        {
            Id = new Guid();
            Views = 1;
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
            Status = true;
            Featured = false;
        }
    }
}
