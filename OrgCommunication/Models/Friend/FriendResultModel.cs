using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Friend
{
    public class FriendResultModel : ResultModel
    {
        public IList<Friend.FriendMemberModel> Friends { get; set; }
    }
}