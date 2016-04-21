using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers.Security;
using OrgCommunication.Models;
using OrgCommunication.Models.Message;
using System;
using System.Linq;
using System.Web.Http;

namespace OrgCommunication.APIs
{
    /// <summary>
    /// Message API
    /// </summary>
    public class MessageController : ApiController
    {
        /// <summary>
        /// Add message for offline member 
        /// </summary>
        /// <param name="param">Offline message add Request Model</param>
        /// <remarks></remarks>
        [HttpPost]
        [Authorize]
        public ResultModel AddOfflineMessage(MessageOfflineAddRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                MessageBL bl = new MessageBL();

                bl.AddOfflineMessage(param);

                result.Status = true;
                result.Message = "Offline message is added";
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
        /// Update offline message (data or type)
        /// </summary>
        /// <param name="param">Offline message update Request Model</param>
        /// <remarks></remarks>
        [HttpPost]
        [Authorize]
        public ResultModel UpdateOfflineMessage(MessageOfflineUpdateRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                MessageBL bl = new MessageBL();

                bl.UpdateOfflineMessage(param);

                result.Status = true;
                result.Message = "Offline message is updated";
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
        /// Get offline messages (the return messages CANNOT be retrieved again)
        /// </summary>
        /// <remarks></remarks>
        [HttpPost]
        [Authorize]
        public OfflineMessageResultModel GetOfflineMessages()
        {
            OfflineMessageResultModel result = new OfflineMessageResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                MessageBL bl = new MessageBL();

                var messages = bl.GetOfflineMessageByMemberId(memberId.Value);

                result.Status = true;
                result.Message = "Found " + messages.Count.ToString() + " messages";
                result.OfflineMessages = messages;
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