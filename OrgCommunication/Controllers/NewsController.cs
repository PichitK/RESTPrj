using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers.Drawing;
using System;
using System.Web;
using System.Web.Mvc;

namespace OrgCommunication.Controllers
{
    public class NewsController : Business.Abstract.BaseController
    {
        public FileResult Image(Models.News.NewsRequesetModel param)
        {
            try
            {
                if (param == null || !param.Id.HasValue)
                    throw new OrgException("Invalid News Id");

                NewsBL bl = new NewsBL();

                var image = bl.GetImageNewsContentById(param.Id.Value);
                
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