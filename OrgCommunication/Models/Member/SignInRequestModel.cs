using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Member
{
    public class SignInRequestModel
    {
        //[Display(Name = "User name")]
        //public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Display(Name = "Phone No")]
        //public string Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public int? DeviceOSId { get; set; }
        public string DeviceToken { get; set; } 
    }
}