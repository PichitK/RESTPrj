using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Member
{
    public class DeviceTokenRequestModel
    {
        public int? OSId { get; set; }
        public string Token { get; set; }
    }
}