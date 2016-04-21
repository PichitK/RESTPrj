using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Note
{
    public class NoteImageCreateRequestModel
    {
        public MultipartDataMediaFormatter.Infrastructure.HttpFile Image { get; set; }
    }
}