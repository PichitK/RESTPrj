
using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers.Security;
using OrgCommunication.Models;
using OrgCommunication.Models.Note;
using System;
using System.Linq;
using System.Web.Http;

namespace OrgCommunication.APIs
{
    /// <summary>
    /// Note API
    /// </summary>
    public class NoteController : ApiController
    {
        /// <summary>
        /// Create note (text type)
        /// </summary>
        /// <param name="param">Create Note message Request Model</param>
        /// <remarks></remarks>
        [Authorize]
        public NoteResultModel CreateTextNote(NoteMessageCreateRequestModel param)
        {
            NoteResultModel result = new NoteResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                NoteBL bl = new NoteBL();

                var note = bl.CreateNoteMessage(memberId.Value, param);

                result.Status = true;
                result.Message = "Note created";
                result.Note = note;
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
        /// Create note (image type)
        /// </summary>
        /// <param name="param">Create Note Image Request Model</param>
        /// <remarks></remarks>
        [SwaggerConfig.SwashConsumeMultipart(typeof(NoteImageCreateRequestModel))]
        [Authorize]
        public NoteResultModel CreateImageNote(NoteImageCreateRequestModel param)
        {
            NoteResultModel result = new NoteResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                NoteBL bl = new NoteBL();

                var note = bl.CreateNoteImage(memberId.Value, param);

                result.Status = true;
                result.Message = "Note created";
                result.Note = note;
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
        /// Update note (set new text or change from image to text)
        /// </summary>
        /// <param name="param">Update Note message Request Model</param>
        /// <remarks></remarks>
        [Authorize]
        public NoteResultModel UpdateTextNote(NoteMessageUpdateRequestModel param)
        {
            NoteResultModel result = new NoteResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                NoteBL bl = new NoteBL();

                var note = bl.UpdateNoteMessage(memberId.Value, param);

                result.Status = true;
                result.Message = "Note updated";
                result.Note = note;
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
        /// Update note (set new image or change from text to image)
        /// </summary>
        /// <param name="param">Update Note Image Request Model</param>
        /// <remarks></remarks>
        [SwaggerConfig.SwashConsumeMultipart(typeof(NoteImageUpdateRequestModel))]
        [Authorize]
        public NoteResultModel UpdateImageNote(NoteImageUpdateRequestModel param)
        {
            NoteResultModel result = new NoteResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                NoteBL bl = new NoteBL();

                var note = bl.UpdateNoteImage(memberId.Value, param);

                result.Status = true;
                result.Message = "Note updated";
                result.Note = note;
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
        /// Get notes (all types)
        /// </summary>
        /// <remarks></remarks>
        [HttpPost]
        [Authorize]
        public MultipleNoteResultModel GetNotes()
        {
            MultipleNoteResultModel result = new MultipleNoteResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                NoteBL bl = new NoteBL();

                var notes = bl.GetNotesByMember(memberId.Value);

                result.Status = true;
                result.Message = "Found " + notes.Count.ToString() + " notes";
                result.Notes = notes;
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
        /// Delete note
        /// </summary>
        /// <param name="param">Note Delete Request Model</param>
        /// <remarks></remarks>
        [HttpPost]
        [Authorize]
        public ResultModel DeleteNote(NoteDeleteRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                if ((param == null) || !param.noteId.HasValue)
                    throw new OrgException("Invalid note Id");


                NoteBL bl = new NoteBL();

                bl.RemoveNoteById(memberId, param.noteId.Value);

                result.Status = true;
                result.Message = "Deleted note successfully";
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