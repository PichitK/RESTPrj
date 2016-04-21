using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Member
{
    public class ActivateRequestModel
    {
        public int MemberId { get; set; }
        public string ActivationKey { get; set; }
        public int? DeviceOSId { get; set; }
        public string DeviceToken { get; set; }
    }
}