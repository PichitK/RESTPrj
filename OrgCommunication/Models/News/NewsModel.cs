using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.News
{
    public class NewsModel
    {
        public int Id { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public string CreatedDate { get; set; }
        public long Ticks { get; set; }

        public IList<NewsLikeModel> Likes { get; set; }
        public IList<string> Text { get; set; }
        public IList<string> ImageUrl { get; set; }
    }
}