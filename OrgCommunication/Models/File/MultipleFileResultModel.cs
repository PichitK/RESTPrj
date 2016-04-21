using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.File
{
    /// <summary>
    /// Multiple file Result
    /// </summary>
    public class MultipleFileResultModel : ResultModel
    {
        public IList<FileModel> Files { get; set; }
    }
}