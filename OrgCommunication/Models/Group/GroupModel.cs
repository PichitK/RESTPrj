using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Group
{
    public class GroupModel
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string TypeDescription { get; set; }
        public int FounderId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string WelcomeMessage { get; set; }
        public string Logo { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string RoomId { get; set; }

        public IList<Member.MemberModel> Members { get; set; }
    }
}