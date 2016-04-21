using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.News
{
    public class NewsCommentByCriteriaRequestModel
    {
        public int? NewsId { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}