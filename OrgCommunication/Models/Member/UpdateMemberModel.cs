using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Member
{
    public class UpdateMemberModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string DisplayName { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public MultipartDataMediaFormatter.Infrastructure.HttpFile Photo { get; set; }
    }
}