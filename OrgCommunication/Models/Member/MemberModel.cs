﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Member
{
    /// <summary>
    /// Member Information
    /// </summary>
    public class MemberModel
    {
        /// <summary>
        /// Member MemberId
        /// </summary>
        public Int32 Id { get; set; }
        public string FacebookId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string DisplayName { get; set; }
        public string Gender { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string EmployeeId { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public string AccessToken { get; set; }
    }
}
