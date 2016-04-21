using OrgCommunication.Models.Member;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Member
{
    public class SignInResultModel : ResultModel
    {
        public MemberModel Member { get; set; }
    }
}