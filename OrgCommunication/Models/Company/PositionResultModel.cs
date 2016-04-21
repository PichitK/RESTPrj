using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Company
{
    public class PostionResultModel : ResultModel
    {
        public IList<PositionModel> Positions { get; set; }
    }

    public class PositionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }
}