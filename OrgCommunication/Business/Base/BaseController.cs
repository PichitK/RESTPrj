using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers.Drawing;
using System.Web.Mvc;

namespace OrgCommunication.Business.Abstract
{
    public abstract class BaseController : Controller
    {
        #region Private Method
        protected FileResult CreateImageFileResult(byte[] image)
        {
            if (image == null)
                throw new OrgException("No image was set");

            string contentType = "";

            System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();

            using (System.Drawing.Image img = (System.Drawing.Image)converter.ConvertFrom(image))
            {
                string imageType = ImageHelper.GetImageFormat(img);
                if (imageType != null)
                    contentType = "image/" + imageType.ToLower();
            }

            return File(image, contentType);
        }
        #endregion
    }
}