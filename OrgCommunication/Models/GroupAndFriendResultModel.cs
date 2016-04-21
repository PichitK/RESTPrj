using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models
{
    public class GroupAndFriendResultModel : ResultModel
    {
        public IList<Friend.FriendMemberModel> Favourites { get; set; }
        public IList<Friend.FriendMemberModel> Friends { get; set; }
        public IList<Group.GroupModel> Groups { get; set; }
    }
}