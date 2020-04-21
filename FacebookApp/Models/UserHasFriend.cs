using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookApp.Models
{
    public class UserHasFriend
    {
        [Key, Column(Order = 1)]
        public string UserId { get; set; }

        [ForeignKey("User")]
        [Key, Column(Order = 2)]
        public string FriendId { get; set; }

        public FriendRequestStatus Status { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
