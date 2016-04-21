using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Group
{
    public class MultipleGroupResultModel : ResultModel
    {
        public IList<GroupModel> Groups { get; set; }
    }
}