using FacebookApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookApp.ViewModel
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Posts = new HashSet<PostsViewModel>();
            //Friends = new HashSet<User>();
            //FriendShipStatus = new HashSet<UserHasFriend>();
            Friends = new HashSet<FriendsViewModel>();
        }
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Bio { get; set; }
        public Gender Gender { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool? isBlocked { get; set; } = false;
        public string Role { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }

        public virtual ICollection<PostsViewModel> Posts { get; set; }
        public virtual ICollection<FriendsViewModel> Friends { get; set; }
        //public virtual ICollection<User> Friends { get; set; }
        //public virtual ICollection<UserHasFriend> FriendShipStatus { get; set; }
    }
}
