using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Note
{
    public class MultipleNoteResultModel : ResultModel
    {
        public IList<NoteModel> Notes { get; set; }
    }
}