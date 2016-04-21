using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Models;
using OrgCommunication.Models.Sticker;
using System;
using System.Linq;
using System.Web.Http;

namespace OrgCommunication.APIs
{
    /// <summary>
    /// Sticker API
    /// </summary>
    public class StickerController : ApiController
    {
        /// <summary>
        /// Create sticker (for test)
        /// </summary>`
        /// <param name="param">Create Sticker Request Model</param>
        /// <remarks></remarks>
        [HttpPost]
        [SwaggerConfig.SwashConsumeMultipart(typeof(StickerCreateRequestModel))]
        public ResultModel CreateNews(StickerCreateRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                StickerBL bl = new StickerBL();

                bl.CreateSticker(param);

                result.Status = true;
                result.Message = "Sticker created";
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
        /// Get all stickers
        /// </summary>
        /// <remarks>
        /// To get sticker image , go to [host]/Sticker/image?Id={stickerImageId}
        /// </remarks>
        [HttpPost]
        [Authorize]
        public StickerResultModel GetStickers()
        {
            StickerResultModel result = new StickerResultModel();

            try
            {
                StickerBL bl = new StickerBL();

                var stickers = bl.GetStickerPackageById(null);

                result.Status = true;
                result.Message = "Got " + stickers.Count.ToString() + " stickers";
                result.Stickers = stickers;

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