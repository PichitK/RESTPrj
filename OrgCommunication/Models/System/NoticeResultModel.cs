using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.System
{
    public class NoticeResultModel : ResultModel
    {
        public IList<NoticeModel> Notices { get; set; }
    }

    public class NoticeModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreatedDate { get; set; }
    }
}