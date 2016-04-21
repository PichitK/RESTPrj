using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Group
{
    public class AddGroupMemberRequestModel
    {
        public int? GroupId { get; set; }
        public int? MemberId { get; set; }
    }
}