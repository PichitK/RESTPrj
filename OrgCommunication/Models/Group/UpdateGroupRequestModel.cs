using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Group
{
    public class UpdateGroupRequestModel
    {
        public int? GroupId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string WelcomeMessage { get; set; }
        public MultipartDataMediaFormatter.Infrastructure.HttpFile Logo { get; set; }
    }
}