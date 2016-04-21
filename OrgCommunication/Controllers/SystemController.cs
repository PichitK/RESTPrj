using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using System;
using System.Web;
using System.Web.Mvc;

namespace OrgCommunication.Controllers
{
    public class SystemController : Controller
    {
        public ActionResult Notice(Models.System.NoticeRequesetModel param)
        {
            try
            {
                if (param == null || !param.Id.HasValue)
                    throw new OrgException("Invalid notice Id");

                SystemBL bl = new SystemBL();

                var notice = bl.GetNoticeById(param.Id.Value);
                
                return View(notice);
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