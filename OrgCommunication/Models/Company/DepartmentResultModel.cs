using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Models.Company
{
    public class DepartmentResultModel : ResultModel
    {
        public IList<DepartmentModel> Departments { get; set; }
    }

    public class DepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }
}