using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookApp.Models
{
    public class UserCommentsOnPost
    {
        [Key]
        public int CommentId { get; set; }

        //[Key, Column(Order = 1)]
        public string UserId { get; set; }

        //[Key, Column(Order = 2)]
        public int PostId { get; set; }

        public DateTime CommentDate { get; set; } = DateTime.Now;

        [Required]
        public string Content { get; set; }

        public bool IsDeleted { get; set; } = false;

        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
    }
}
