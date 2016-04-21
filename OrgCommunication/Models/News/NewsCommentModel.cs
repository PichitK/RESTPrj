using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.News
{
    public class NewsCommentModel
    {
        public int MemberId { get; set; }
        public int Id { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }
        public string CreatedDate { get; set; }
        public long Ticks { get; set; }
    }
}