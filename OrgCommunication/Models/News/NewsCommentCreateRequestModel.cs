using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.News
{
    public class NewsCommentCreateRequesetModel
    {
        public int? NewsId { get; set; }
        public string Text { get; set; }
        public int? Type { get; set; }
    }
}