using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookApp.ViewModel
{
    public class PostsViewModel
    {
        public string Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostingDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; }

        public string UserId { get; set; }
        //update
        public string Image { get; set; }

        public IFormFile ImageFile { get; set; }
        public int PostId { get; set; }

        public IEnumerable<LikesViewModel> likesVMList;

        public IEnumerable<CommentsViewModel> commentsVMList;

        //public string Like_UserId { get; set; }
        //public int Like_PostId { get; set; }

        //public bool Like_IsLiked { get; set; }

        //public string Image { get; set; } = "~/Images/defaultMaleUserImage.jpg";

        //public string Username { get; set; }
        //public string Nickname { get; set; }

        //public string Email { get; set; }
    }
}
