using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Friend
{
    public class FriendHiddenRequestModel
    {
        public int? FriendMemberId { get; set; }
        public bool IsHidden { get; set; }
    }
}