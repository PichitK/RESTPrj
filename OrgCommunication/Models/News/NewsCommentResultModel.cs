using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.News
{
    public class NewsCommentResultModel : ResultModel
    {
        public IList<NewsCommentModel> Comments { get; set; }
    }
}