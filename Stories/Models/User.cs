using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Stories.Models
{
    [Table("User", Schema = "public")]
    public class User
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("name")]
        public string Name { get; set; }        
        [Column("email")]
        public string Email { get; set; }
        [Column("avatar")]
        public string Avatar { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("is_author")]
        public bool IsAuthor { get; set; }
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
        [Column("facebook")]
        public string Facebook { get; set; }
        [Column("twitter")]
        public string Twitter { get; set; }
        [Column("instagram")]
        public string Instagram { get; set; }
        [Column("pinterest")]
        public string Pinterest { get; set; }

        public User()
        {
            Id = new Guid();
            IsAuthor = false;
            Description = "";
            CreatedDate = DateTime.Now;
        }
    }
}
