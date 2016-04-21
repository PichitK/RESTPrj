using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Note
{
    public class NoteModel
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string TypeDescription { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public string CreatedDate { get; set; }
        public long Ticks { get; set; }
    }
}