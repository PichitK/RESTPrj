using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Message
{
    public class MessageOfflineUpdateRequestModel
    {
        public int? messageId { get; set; }
        public string Data { get; set; } 
        public int? Type { get; set; }
    }
}