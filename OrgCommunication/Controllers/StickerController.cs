using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers.Drawing;
using System;
using System.Web;
using System.Web.Mvc;

namespace OrgCommunication.Controllers
{
    public class StickerController : Business.Abstract.BaseController
    {
        public FileResult Thumbnail(Models.Sticker.StickerPackageRequestModel param)
        {
            try
            {
                if (param == null || !param.Id.HasValue)
                    throw new OrgException("Invalid sticker package Id");

                StickerBL bl = new StickerBL();

                var thumbnail = bl.GetStickerThumbnailById(param.Id.Value);
                
                return this.CreateImageFileResult(thumbnail);
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

        public FileResult Zip(Models.Sticker.StickerPackageRequestModel param)
        {
            try
            {
                if (param == null || !param.Id.HasValue)
                    throw new OrgException("Invalid sticker package Id");

                StickerBL bl = new StickerBL();

                var stickerItems = bl.GetStickerItemByPackageId(param.Id.Value);

                if (stickerItems.Count == 0)
                    throw new OrgException("Sticker package not found");

                byte[] zipData = null;

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (var zip = new Ionic.Zip.ZipFile())
                    {
                        foreach (var item in stickerItems)
                        {
                            zip.AddEntry(item.Filename, item.Image);
                        }

                        zip.Save(ms);
                    }

                    zipData = ms.ToArray();
                }

                return File(zipData, "application/zip", param.Id.Value.ToString() + ".zip");
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

        public FileResult Image(Models.Sticker.StickerItemRequestModel param)
        {
            try
            {
                if (param == null || !param.Id.HasValue)
                    throw new OrgException("Invalid News Id");

                StickerBL bl = new StickerBL();

                var image = bl.GetStickerItemImageById(param.Id.Value);

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