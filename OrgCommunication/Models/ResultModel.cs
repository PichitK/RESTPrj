using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models
{
    public class ResultModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string InternalMessage { get; set; }
    }
}