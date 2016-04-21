using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers.Drawing;
using System;
using System.Web;
using System.Web.Mvc;

namespace OrgCommunication.Controllers
{
    public class FileController : Controller
    {
        [HttpGet]
        public FileResult Download(string id)
        {
            try
            {
                UploadBL bl = new UploadBL();

                var file = bl.GetFileContentById(id);

                if (file == null)
                    throw new OrgException("Invalid file");

                return File(file.Date, file.MediaType, file.Filename);
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