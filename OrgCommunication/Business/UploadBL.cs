using System.Linq;
using OrgCommunication.Models.File;
using System.Collections.Generic;
using OrgComm.Data;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using System;
using System.Web;

namespace OrgCommunication.Business
{
    public class UploadBL
    {
        private static string _fileUrlFormatString = null;
        public static string FileUrlFormatString
        {
            get
            {
                string url = null;

                if (System.Web.HttpContext.Current == null)
                {
                    return "/File/Download?Id={0}";
                }
                else
                {
                    if (_fileUrlFormatString == null)
                        _fileUrlFormatString = string.Format("{0}{1}{2}",
                        HttpContext.Current.Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped),
                        (HttpContext.Current.Request.ApplicationPath.Equals("/")) ? "/" : HttpContext.Current.Request.ApplicationPath
                        , "File/Download?Id={0}");

                    return _fileUrlFormatString;
                }
            }
        }

        public UploadBL()
        {

        }

        public FileModel AddFile(int memberId, FileUploadRequestModel model, OrgComm.Data.Models.Upload.UploadType type)
        {
            if (model.File == null)
                throw new OrgException("Invalid upload file");

            OrgComm.Data.Models.Upload upload = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                upload = new OrgComm.Data.Models.Upload();

                upload.Id = Guid.NewGuid().ToString("N");

                //if key is already existed, re-generate
                while (dbc.Uploads.Any(r => r.Id.Equals(upload.Id)))
                {
                    upload.Id = Guid.NewGuid().ToString("N");
                }
                
                upload.MemberId = memberId;
                upload.File = model.File.Buffer;
                upload.Size = model.File.Buffer.LongLength;
                upload.Type = (int)type;
                upload.Filename = model.File.FileName;
                upload.MediaType = model.File.MediaType;
                upload.CreatedDate = DateTime.Now;

                dbc.Uploads.Add(upload);

                dbc.SaveChanges();
            }

            return new FileModel
            {
                Id = upload.Id,
                Url = UploadBL.FileUrlFormatString.Replace("{0}", upload.Id),
                Filename = upload.Filename,
                Size = upload.File.LongLength,
                CreatedDate = upload.CreatedDate.ToString(AppConfigs.GeneralDateTimeFormat),
                Ticks = upload.CreatedDate.Ticks
            };
        }

        public void RemoveFile(int? memberId, string fileId)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                OrgComm.Data.Models.Upload upload = dbc.Uploads.SingleOrDefault(r => r.Id.Equals(fileId));

                if (upload == null)
                    throw new OrgException("File not found");

                //validate owner if member Id is provided
                if(memberId.HasValue)
                {
                    if (memberId.Value != upload.MemberId)
                        throw new OrgException("No authorization to remove file");
                }

                dbc.Uploads.Remove(upload);

                dbc.SaveChanges();
            }
        }

        public FileContentModel GetFileContentById(string id)
        {
            OrgComm.Data.Models.Upload upload = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                upload = dbc.Uploads.SingleOrDefault(r => r.Id.Equals(id));

                if (upload == null)
                    throw new OrgException("File not found");
            }

            return new FileContentModel
            {
                Filename = upload.Filename,
                MediaType = upload.MediaType,
                Date = upload.File
            };
        }

        public List<FileModel> GetFilesByMemberId(int memberId)
        {
            List<FileModel> uploadList = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                uploadList = dbc.Uploads.Where(r => r.MemberId.Equals(memberId)).ToList().Select(r => new FileModel
                {
                    Id = r.Id,
                    Url = UploadBL.FileUrlFormatString.Replace("{0}", r.Id),
                    Filename = r.Filename,
                    Size = r.Size,
                    CreatedDate = r.CreatedDate.ToString(AppConfigs.GeneralDateTimeFormat),
                    Ticks = r.CreatedDate.Ticks
                }).ToList();
            }

            return uploadList;
        }

        public void RemoveFilesByMemberId(int memberId)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var uploads = dbc.Uploads.Where(r => r.MemberId.Equals(memberId));
                if (uploads.Count() > 0)
                    dbc.Uploads.RemoveRange(uploads);
                
                dbc.SaveChanges();
            }
        }
    }
}