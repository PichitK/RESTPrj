using System.Linq;
using OrgCommunication.Models.Sticker;
using System.Collections.Generic;
using OrgComm.Data;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using System;
using System.Web;

namespace OrgCommunication.Business
{
    public class StickerBL
    {
        private static string _thumbnailUrlFormatString = null;
        public static string ThumbnailUrlFormatString
        {
            get
            {
                string url = null;

                if (System.Web.HttpContext.Current == null)
                {
                    return "/Sticker/Thumbnail?Id={0}";
                }
                else
                {
                    if (_thumbnailUrlFormatString == null)
                        _thumbnailUrlFormatString = string.Format("{0}{1}{2}",
                        HttpContext.Current.Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped),
                        (HttpContext.Current.Request.ApplicationPath.Equals("/")) ? "/" : HttpContext.Current.Request.ApplicationPath
                        , "Sticker/Thumbnail?Id={0}");

                    return _thumbnailUrlFormatString;
                }
            }
        }

        private static string _zipUrlFormatString = null;
        public static string ZipUrlFormatString
        {
            get
            {
                string url = null;

                if (System.Web.HttpContext.Current == null)
                {
                    return "/Sticker/Zip?Id={0}";
                }
                else
                {
                    if (_zipUrlFormatString == null)
                        _zipUrlFormatString = string.Format("{0}{1}{2}",
                        HttpContext.Current.Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped),
                        (HttpContext.Current.Request.ApplicationPath.Equals("/")) ? "/" : HttpContext.Current.Request.ApplicationPath
                        , "Sticker/Zip?Id={0}");

                    return _zipUrlFormatString;
                }
            }
        }

        public StickerBL()
        {

        }

        public void CreateSticker(StickerCreateRequestModel model)
        {
            if ((model == null))
                throw new OrgException("Invalid sticker");

            if (String.IsNullOrWhiteSpace(model.Title))
                throw new OrgException("Invalid sticker title");

            if (model.Thumbnail == null)
                throw new OrgException("Invalid sticker thumbnail");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                OrgComm.Data.Models.StickerPackage sticker = new OrgComm.Data.Models.StickerPackage
                {
                    Title = model.Title,
                    Description = model.Description,
                    Thumbnail = model.Thumbnail.Buffer,
                    CreatedDate = DateTime.Now,
                    Items = new List<OrgComm.Data.Models.StickerItem>()
                };

                //Temporarily
                #region Add image type
                if (model.Image1 != null)
                {
                    sticker.Items.Add(new OrgComm.Data.Models.StickerItem { Image = model.Image1.Buffer, Extension = System.IO.Path.GetExtension(model.Image1.FileName) });
                }

                if (model.Image2 != null)
                {
                    sticker.Items.Add(new OrgComm.Data.Models.StickerItem { Image = model.Image2.Buffer, Extension = System.IO.Path.GetExtension(model.Image1.FileName) });
                }

                if (model.Image3 != null)
                {
                    sticker.Items.Add(new OrgComm.Data.Models.StickerItem { Image = model.Image3.Buffer, Extension = System.IO.Path.GetExtension(model.Image1.FileName) });
                }

                if (model.Image4 != null)
                {
                    sticker.Items.Add(new OrgComm.Data.Models.StickerItem { Image = model.Image4.Buffer, Extension = System.IO.Path.GetExtension(model.Image1.FileName) });
                }

                if (model.Image5 != null)
                {
                    sticker.Items.Add(new OrgComm.Data.Models.StickerItem { Image = model.Image5.Buffer, Extension = System.IO.Path.GetExtension(model.Image1.FileName) });
                }
                #endregion
                //-------end temporarily

                dbc.Stickers.Add(sticker);

                dbc.SaveChanges();
            }
        }

        public IList<StickerModel> GetStickerPackageById(int? stickerId)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var qry = dbc.Stickers.AsQueryable();

                if (stickerId.HasValue)
                    qry = qry.Where(r => r.Id == stickerId.Value);

                return qry.ToList().Select(r => new StickerModel
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    Status = (DateTime.Now.Subtract(r.CreatedDate.Date).Days > 14) ? "Old" : "New", //old, if more than 2 weeks
                    Thumbnail = StickerBL.ThumbnailUrlFormatString.Replace("{0}", r.Id.ToString()),
                    Zip = StickerBL.ZipUrlFormatString.Replace("{0}", r.Id.ToString()),
                    Items = r.Items.Select(rr => rr.Id).ToList()
                }).ToList();
            }
        }

        public IList<StickerItemModel> GetStickerItemByPackageId(int? stickerId)
        {
            if (!stickerId.HasValue)
                throw new OrgException("Invalid sticker package");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var qry = dbc.StickerItems.AsQueryable();

                if (stickerId.HasValue)
                    qry = qry.Where(r => r.StickerId == stickerId.Value);

                return qry.Select(r => new StickerItemModel
                {
                    Filename = r.Id.ToString() + r.Extension,
                    Image = r.Image
                }).ToList();
            }
        }

        public byte[] GetStickerThumbnailById(int? stickerId)
        {
            if (!stickerId.HasValue)
                throw new OrgException("Invalid sticker Id");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                byte[] image = dbc.Stickers.Where(r => (r.Id == stickerId.Value)).Select(r => r.Thumbnail).FirstOrDefault();

                if (image == null)
                    throw new OrgException("Thumbnail not found");

                return image;
            }
        }

        public byte[] GetStickerItemImageById(int stickerItemId)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                byte[] image = dbc.StickerItems.Where(r => (r.Id == stickerItemId)).Select(r => r.Image).FirstOrDefault();

                if (image == null)
                    throw new OrgException("Image not found");

                return image;
            }
        }
    }
}