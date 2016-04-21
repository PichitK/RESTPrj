using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Member
{
    public class SignInWithFacebookRequestModel
    {
        public string FacebookId { get; set; }
        public int? DeviceOSId { get; set; }
        public string DeviceToken { get; set; } 
    }
}