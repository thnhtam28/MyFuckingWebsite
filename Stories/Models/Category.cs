using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Models
{
    [Table("Category", Schema = "public")]
    public class Category
    {
        [Column("id")]
        public string Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("color")]
        public string Color { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("thumb_link")]
        public string ThumbLink { get; set; }
    }
}
