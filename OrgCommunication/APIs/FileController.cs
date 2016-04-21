using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers.Security;
using OrgCommunication.Models;
using OrgCommunication.Models.File;
using System;
using System.Linq;
using System.Web.Http;

namespace OrgCommunication.APIs
{
    /// <summary>
    /// File API
    /// </summary>
    public class FileController : ApiController
    {
        /// <summary>
        /// Upload video
        /// </summary>
        [Authorize]
        [HttpPost]
        [SwaggerConfig.SwashConsumeMultipart(typeof(FileUploadRequestModel))]
        public FileResultModel UploadVideo(FileUploadRequestModel param)
        {
            FileResultModel result = new FileResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                UploadBL bl = new UploadBL();

                var file = bl.AddFile(memberId.Value, param, OrgComm.Data.Models.Upload.UploadType.Video);

                result.Status = true;
                result.File = file;
                result.Message = "Uploaded successfully";
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
        /// Upload audio
        /// </summary>
        [Authorize]
        [HttpPost]
        [SwaggerConfig.SwashConsumeMultipart(typeof(FileUploadRequestModel))]
        public FileResultModel UploadAudio(FileUploadRequestModel param)
        {
            FileResultModel result = new FileResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                UploadBL bl = new UploadBL();

                var file = bl.AddFile(memberId.Value, param, OrgComm.Data.Models.Upload.UploadType.Audio);

                result.Status = true;
                result.File = file;
                result.Message = "Uploaded successfully";
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
        /// Upload photo
        /// </summary>
        [Authorize]
        [HttpPost]
        [SwaggerConfig.SwashConsumeMultipart(typeof(FileUploadRequestModel))]
        public FileResultModel UploadPhoto(FileUploadRequestModel param)
        {
            FileResultModel result = new FileResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                UploadBL bl = new UploadBL();

                var file = bl.AddFile(memberId.Value, param, OrgComm.Data.Models.Upload.UploadType.Photo);

                result.Status = true;
                result.File = file;
                result.Message = "Uploaded successfully";
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
        /// Upload file
        /// </summary>
        [Authorize]
        [HttpPost]
        [SwaggerConfig.SwashConsumeMultipart(typeof(FileUploadRequestModel))]
        public FileResultModel UploadFile(FileUploadRequestModel param)
        {
            FileResultModel result = new FileResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                UploadBL bl = new UploadBL();

                var file = bl.AddFile(memberId.Value, param, OrgComm.Data.Models.Upload.UploadType.Other);

                result.Status = true;
                result.File = file;
                result.Message = "Uploaded successfully";
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
        /// Delete file
        /// </summary>
        /// <param name="param">File Delete Request Model</param>
        /// <remarks></remarks>
        [HttpPost]
        [Authorize]
        public ResultModel DeleteFile(FileDeleteRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                if ((param == null) || String.IsNullOrWhiteSpace(param.fileId))
                    throw new OrgException("Invalid file Id");

                UploadBL bl = new UploadBL();

                bl.RemoveFile(memberId, param.fileId);

                result.Status = true;
                result.Message = "Delete file successfully";
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
        /// Get uploaded files
        /// </summary>
        [Authorize]
        [HttpPost]
        public MultipleFileResultModel GetFiles()
        {
            MultipleFileResultModel result = new MultipleFileResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                UploadBL bl = new UploadBL();

                var files = bl.GetFilesByMemberId(memberId.Value);

                result.Status = true;
                result.Files = files;
                result.Message = result.Files.Count.ToString("#,##0") + " files";
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