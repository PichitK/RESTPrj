using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Sticker
{
    public class StickerCreateRequestModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public MultipartDataMediaFormatter.Infrastructure.HttpFile Thumbnail { get; set; }
        public MultipartDataMediaFormatter.Infrastructure.HttpFile Image1 { get; set; }
        public MultipartDataMediaFormatter.Infrastructure.HttpFile Image2 { get; set; }
        public MultipartDataMediaFormatter.Infrastructure.HttpFile Image3 { get; set; }
        public MultipartDataMediaFormatter.Infrastructure.HttpFile Image4 { get; set; }
        public MultipartDataMediaFormatter.Infrastructure.HttpFile Image5 { get; set; }
    }
}