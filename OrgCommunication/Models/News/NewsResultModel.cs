using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.News
{
    public class NewsResultModel : ResultModel
    {
        public IList<NewsModel> News { get; set; }
    }
}