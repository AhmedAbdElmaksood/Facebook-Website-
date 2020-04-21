using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookApp.Models
{
    public class Post
    {
        public Post()
        {
            UserLikesPosts = new HashSet<UserLikesPost>();
            UserCommentsOnPosts = new HashSet<UserCommentsOnPost>();
        }

        [Key]
        public int Id { get; set; }
        
        public string Content { get; set; }
        public string Image { get; set; }
        [DataType(DataType.Date)]
        public DateTime PostingDate { get; set; } = DateTime.Now;
        
        public bool IsDeleted { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<UserLikesPost> UserLikesPosts { get; set; }
        public virtual ICollection<UserCommentsOnPost> UserCommentsOnPosts { get; set; }
    }
}
