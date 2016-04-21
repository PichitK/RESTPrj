using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Message
{
    public class MessageOfflineAddRequestModel
    {
        public int? ToMemberId { get; set; }
        public string Data { get; set; }
        public int? Type { get; set; }

    }
}