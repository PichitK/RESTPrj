using System.Linq;
using OrgComm.Data;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using System;
using System.Text;
using OrgCommunication.Models.System;
using System.Collections.Generic;

namespace OrgCommunication.Business
{
    public class SystemBL
    {
        public SystemBL()
        {

        }

        public void CreateNotice(NoticeCreateRequestModel model)
        {
            if (model == null)
                throw new OrgException("Invalid notice");

            if (String.IsNullOrWhiteSpace(model.Title))
                throw new OrgException("Invalid title");

            if (String.IsNullOrWhiteSpace(model.Content))
                throw new OrgException("Invalid content");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                OrgComm.Data.Models.Notice notice = new OrgComm.Data.Models.Notice
                {
                    Title = model.Title,
                    Content = model.Content,
                    CreatedBy = 1, //TEMP
                    CreatedDate = DateTime.Now,
                };

                dbc.Notices.Add(notice);

                dbc.SaveChanges();
            }
        }

        public NoticeWithContentModel GetNoticeById(int noticeId)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var notice = dbc.Notices.SingleOrDefault(r => r.Id == noticeId);

                if (notice == null)
                    throw new OrgException("Notice not found");

                return new NoticeWithContentModel
                {
                    Id = notice.Id,
                    Title = notice.Title,
                    Content = notice.Content,
                    CreatedDate = notice.CreatedDate.ToString(AppConfigs.GeneralDateTimeFormat)
                };
            }
        }

        public IList<NoticeModel> GetNotices(int? noticeId)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var qry = dbc.Notices.AsQueryable();

                if (noticeId.HasValue)
                    qry = qry.Where(r => r.Id == noticeId.Value);

                var temp = qry.Select(r => new { Id = r.Id,  Title = r.Title,  CreatedDate = r.CreatedDate }).ToList();

                return temp.Select(r => new NoticeModel
                {
                    Id = r.Id,
                    Title = r.Title,
                    CreatedDate = r.CreatedDate.ToString(AppConfigs.GeneralDateTimeFormat)
                }).ToList();
            }
        }
    }
}