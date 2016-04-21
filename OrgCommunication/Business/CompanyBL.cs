using OrgComm.Data;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Models.Company;
using System.Collections.Generic;
using System.Linq;

namespace OrgCommunication.Business
{
    public class CompanyBL
    {
        public CompanyBL()
        {

        }

        public IList<OrgComm.Data.Models.Company> GetCompanies()
        {
            List<OrgComm.Data.Models.Company> companies = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                companies = dbc.Company.ToList();
            }

            return companies;
        }

        public IList<OrgComm.Data.Models.Department> GetDepartments(CompanyInfoRequestModel model)
        { 
            List<OrgComm.Data.Models.Department> departments = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var qry = dbc.Department.AsQueryable();

                if ((model != null) && (model.CompanyId.HasValue))
                    qry = qry.Where(r => r.CompanyId.Equals(model.CompanyId.Value));

                departments = qry.ToList();
            }

            return departments;
        }

        public IList<OrgComm.Data.Models.Position> GetPositions(CompanyInfoRequestModel model)
        {
            List<OrgComm.Data.Models.Position> positions = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var qry = dbc.Position.AsQueryable();

                if ((model != null) && (model.CompanyId.HasValue))
                    qry = qry.Where(r => r.CompanyId.Equals(model.CompanyId.Value));

                positions = qry.ToList();
            }

            return positions;
        }
    }
}