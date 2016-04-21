using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers.Drawing;
using System;
using System.Web;
using System.Web.Mvc;

namespace OrgCommunication.Controllers
{
    public class NoteController : Business.Abstract.BaseController
    {
        public FileResult Image(Models.Note.NoteRequestModel param)
        {
            try
            {
                if (param == null || !param.Id.HasValue)
                    throw new OrgException("Invalid note Id");

                NoteBL bl = new NoteBL();

                var image = bl.GetNoteImage(param);
                
                return this.CreateImageFileResult(image);
            }
            catch (OrgException oex)
            {
                throw new HttpException((int)System.Net.HttpStatusCode.NotFound, oex.Message);
            }
            catch (Exception ex)
            {
                if (AppConfigs.DebugInternalMessage)
                    throw new HttpException((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
                else
                    throw new HttpException((int)System.Net.HttpStatusCode.NotFound, AppConfigs.InternalErrorMessage);
            }
        }
    }
}