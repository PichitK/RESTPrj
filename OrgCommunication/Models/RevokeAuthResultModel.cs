using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models
{
    public class RevokeAuthResultModel : ResultModel
    {
        public string AccessToken { get; set; }
    }
}