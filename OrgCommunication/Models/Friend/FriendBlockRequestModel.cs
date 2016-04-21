using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Friend
{
    public class FriendBlockRequestModel
    {
        public int? FriendMemberId { get; set; }
        public bool IsBlocked { get; set; }
    }
}