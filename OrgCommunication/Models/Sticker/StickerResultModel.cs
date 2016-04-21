using OrgCommunication.Models.Sticker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Sticker
{
    public class StickerResultModel : ResultModel
    {
        public IList<StickerModel> Stickers { get; set; }
    }
}