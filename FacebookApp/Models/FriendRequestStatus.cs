using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookApp.Models
{
    public enum FriendRequestStatus
    {
        Pending = 1,
        Friend = 2,
        NotFriend = 3,


        SentAndPending = 4, //1,
        //Friend = 2,
        //NotFriend = 3,
        ReceivedAndPending = 5 //4
    }
}
