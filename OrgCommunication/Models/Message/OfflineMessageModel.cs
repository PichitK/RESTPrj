using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Message
{
    public class OfflineMessageModel
    {

        public int Id { get; set; }
        public string Data { get; set; }
        public int Type { get; set; }
        public string CreatedDate { get; set; }
        public long Ticks { get; set; }
    }
}