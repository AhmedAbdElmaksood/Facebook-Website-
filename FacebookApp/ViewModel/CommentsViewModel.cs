using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookApp.ViewModel
{
    public class CommentsViewModel
    {
        public int CommentId { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CommentDate { get; set; } = DateTime.Now;
        public string Content { get; set; }
        public bool IsDeleted { get; set; }
    }
}
