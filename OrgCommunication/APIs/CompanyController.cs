using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Models.Company;
using System;
using System.Linq;
using System.Web.Http;

namespace OrgCommunication.APIs
{
    /// <summary>
    /// Company API
    /// </summary>
    public class CompanyController : ApiController
    {
        /// <summary>
        /// Get companies
        /// </summary>
        [HttpPost]
        public CompanyResultModel GetCompanies()
        {
            CompanyResultModel result = new CompanyResultModel();

            try
            {
                CompanyBL bl = new CompanyBL();

                var companies = bl.GetCompanies();

                result.Status = true;
                result.Companies = companies.Select(r => new CompanyModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Note = r.Note
                }).ToArray();
                
                result.Message = "Found " + result.Companies.Count.ToString("#,##") + " companies";

            }
            catch (OrgException oex)
            {
                result.Status = false;
                result.Message = oex.Message;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = AppConfigs.InternalErrorMessage;

                if (AppConfigs.DebugInternalMessage)
                    result.InternalMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Get companies
        /// </summary>
        /// <param name="param">Company Information Request Model</param>
        [HttpPost]
        public DepartmentResultModel GetDepartments(CompanyInfoRequestModel param)
        {
            DepartmentResultModel result = new DepartmentResultModel();

            try
            {
                CompanyBL bl = new CompanyBL();

                var departments = bl.GetDepartments(param);

                result.Status = true;
                result.Departments = departments.Select(r => new DepartmentModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Note = r.Note
                }).ToArray();

                result.Message = "Found " + result.Departments.Count.ToString("#,##") + " departments";

            }
            catch (OrgException oex)
            {
                result.Status = false;
                result.Message = oex.Message;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = AppConfigs.InternalErrorMessage;

                if (AppConfigs.DebugInternalMessage)
                    result.InternalMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Get positions of selected company
        /// </summary>
        /// <param name="param">Company Information Request Model</param>
        [HttpPost]
        public PostionResultModel GetPositions(CompanyInfoRequestModel param)
        {
            PostionResultModel result = new PostionResultModel();

            try
            {
                CompanyBL bl = new CompanyBL();

                var positions = bl.GetPositions(param);

                result.Status = true;
                result.Positions = positions.Select(r => new PositionModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Note = r.Note
                }).ToArray();

                result.Message = "Found " + result.Positions.Count.ToString("#,##") + " Positions";

            }
            catch (OrgException oex)
            {
                result.Status = false;
                result.Message = oex.Message;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = AppConfigs.InternalErrorMessage;

                if (AppConfigs.DebugInternalMessage)
                    result.InternalMessage = ex.Message;
            }

            return result;
        }
    }
}