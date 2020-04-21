using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookApp.Models
{
    public class User:IdentityUser
    {
        public User()
        {
            Posts = new HashSet<Post>();
            UserLikesPosts = new HashSet<UserLikesPost>();
            UserCommentsOnPosts = new HashSet<UserCommentsOnPost>();
        }
        public DateTime BirthDate { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        public Gender Gender { get; set; }
        public string Nickname { get; set; }

        public bool? isBlocked { get; set; } = false;
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<UserLikesPost> UserLikesPosts { get; set; }
        public virtual ICollection<UserCommentsOnPost> UserCommentsOnPosts { get; set; }
    }
}
