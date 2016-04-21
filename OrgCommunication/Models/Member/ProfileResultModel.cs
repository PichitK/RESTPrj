using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Member
{
    /// <summary>
    /// Profile Result
    /// </summary>
    public class ProfileResultModel : ResultModel
    {
        public MemberModel Member { get; set; }
    }
}