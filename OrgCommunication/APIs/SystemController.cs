
using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers.Security;
using OrgCommunication.Models;
using OrgCommunication.Models.System;
using System;
using System.Web.Http;

namespace OrgCommunication.APIs
{
    /// <summary>
    /// System API
    /// </summary>
    public class SystemController : ApiController
    {
        /// <summary>
        /// Create News (for test)
        /// </summary>`
        /// <param name="param">Create News Request Model</param>
        /// <remarks></remarks>
        [HttpPost]
        public ResultModel CreateNotice(NoticeCreateRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                SystemBL bl = new SystemBL();

                bl.CreateNotice(param);

                result.Status = true;
                result.Message = "Notice created";
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
        /// Get Notices
        /// </summary>
        /// <remarks>To get content detail, go to [host]/System/Notice?Id={noticeId}</remarks>
        [HttpPost]
        [Authorize]
        public NoticeResultModel GetNotices()
        {
            NoticeResultModel result = new NoticeResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                SystemBL bl = new SystemBL();

                var notices = bl.GetNotices(null);  

                result.Status = true;
                result.Message = "Got " + notices.Count.ToString() + " notices";
                result.Notices = notices;

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