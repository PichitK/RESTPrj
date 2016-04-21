using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Member
{
    public class ExistResultModel : ResultModel
    {
        public IList<ExistModel> Items { get; set; }
    }

    public class ExistModel
    {
        public int? Id { get; set; }
        public bool IsExists { get; set; }
        public string Criteria { get; set; }
    }
}