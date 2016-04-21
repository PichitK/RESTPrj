using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Company
{
    public class CompanyResultModel : ResultModel
    {
        public IList<CompanyModel> Companies { get; set; }
    }

    public class CompanyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }
}