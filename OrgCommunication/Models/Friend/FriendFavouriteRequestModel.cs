using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Friend
{
    public class FriendFavouriteRequestModel
    {
        public int? FriendMemberId { get; set; }
        public bool IsFavourite { get; set; }
    }
}