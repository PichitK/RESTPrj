
using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers.Security;
using OrgCommunication.Models;
using OrgCommunication.Models.News;
using System;
using System.Web.Http;

namespace OrgCommunication.APIs
{
    /// <summary>
    /// News API
    /// </summary>
    public class NewsController : ApiController
    {
        /// <summary>
        /// Create News (for test)
        /// </summary>`
        /// <param name="param">Create News Request Model</param>
        /// <remarks></remarks>
        [HttpPost]
        [SwaggerConfig.SwashConsumeMultipart(typeof(NewsCreateRequestModel))]
        public ResultModel CreateNews(NewsCreateRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                NewsBL bl = new NewsBL();

                bl.CreateNews(1, param);

                result.Status = true;
                result.Message = "News created";
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
        /// Get News
        /// </summary>
        /// <param name="param">News Request Model</param>
        /// <remarks></remarks>
        [HttpPost]
        [Authorize]
        public NewsResultModel GetNews(NewsByCriteriaRequestModel param)
        {
            NewsResultModel result = new NewsResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                NewsBL bl = new NewsBL();

                var news = bl.GetNewsByMember(memberId.Value, param);

                result.Status = true;
                result.Message = "Got " + news.Count.ToString() + " news";
                result.News = news;

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
        /// Set Like for News
        /// </summary>
        /// <param name="param">Set like type on News Request Model; type = null for un-like</param>
        /// <remarks></remarks>
        [HttpPost]
        [Authorize]
        public ResultModel SetLikeForNews(NewsLikeRequesetModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                NewsBL bl = new NewsBL();

                bl.SetLikeForNews(memberId.Value, param);

                result.Status = true;
                result.Message = "Update successfully";
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
        /// Create comment on News
        /// </summary>
        /// <param name="param">Create Note message Request Model</param>
        /// <remarks></remarks>
        [Authorize]
        public ResultModel CreateComment(NewsCommentCreateRequesetModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                NewsBL bl = new NewsBL();

                bl.CreateComment(memberId.Value, param);

                result.Status = true;
                result.Message = "Comment created";
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
        /// Get Comments of selected News
        /// </summary>
        /// <param name="param">News Comment Request Model</param>
        /// <remarks></remarks>
        [HttpPost]
        [Authorize]
        public NewsCommentResultModel GetCommentByNewsId(NewsCommentByCriteriaRequestModel param)
        {
            NewsCommentResultModel result = new NewsCommentResultModel();

            try
            {
                NewsBL bl = new NewsBL();

                var comments = bl.GetCommentsByNewsId(param);

                result.Status = true;
                result.Message = "Got " + comments.Count.ToString() + " comments";
                result.Comments = comments;

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