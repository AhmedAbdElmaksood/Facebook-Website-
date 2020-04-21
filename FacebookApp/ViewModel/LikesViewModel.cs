using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FacebookApp.ViewModel
{
    public class LikesViewModel
    {
        public string UserId { get; set; }
        public int PostId { get; set; }

        public bool IsLiked { get; set; }
    }
}
