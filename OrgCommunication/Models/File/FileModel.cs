using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.File
{
    public class FileModel
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Filename { get; set; }
        public long Size { get; set; }
        public string CreatedDate { get; set; }
        public long Ticks { get; set; }
    }
}