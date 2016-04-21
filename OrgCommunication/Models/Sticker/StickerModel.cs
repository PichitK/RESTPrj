using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Sticker
{
    public class StickerModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Thumbnail { get; set; }
        public string Zip { get; set; }
        public IList<int> Items { get; set; }
    }
}