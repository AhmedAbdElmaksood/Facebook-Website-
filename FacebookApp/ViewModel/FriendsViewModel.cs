using FacebookApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookApp.ViewModel
{
    public class FriendsViewModel
    {
        public string FriendId { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Bio { get; set; }
        public Gender Gender { get; set; }
        public string Nickname { get; set; }
        public FriendRequestStatus Status { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
    }
}
