using System.Linq;
using OrgCommunication.Models.News;
using System.Collections.Generic;
using OrgComm.Data;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using System;
using System.Text;
using System.Web;

namespace OrgCommunication.Business
{
    public class NewsBL
    {
        private static string _imageUrlFormatString = null;
        public static string ImageUrlFormatString
        {
            get
            {
                string url = null;

                if (System.Web.HttpContext.Current == null)
                {
                    return "/News/Image?Id={0}";
                }
                else
                {
                    if (_imageUrlFormatString == null)
                        _imageUrlFormatString = string.Format("{0}{1}{2}",
                        HttpContext.Current.Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped),
                        (HttpContext.Current.Request.ApplicationPath.Equals("/")) ? "/" : HttpContext.Current.Request.ApplicationPath
                        , "News/Image?Id={0}");

                    return _imageUrlFormatString;
                }
            }
        }

        public NewsBL()
        {

        }

        public void CreateNews(int companyId, NewsCreateRequestModel model)
        {
            if ((model == null) || String.IsNullOrWhiteSpace(model.Text))
                throw new OrgException("Invalid note");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                if (!dbc.Company.Any(r => r.Id.Equals(companyId)))
                    throw new OrgException("Invalid company Id");

                OrgComm.Data.Models.News news = new OrgComm.Data.Models.News
                {
                    CompanyId = companyId,
                    CreatedBy = 1, //TEMP
                    CreatedDate = DateTime.Now,
                    LikeCount = 0,
                    Likes = null,
                    CommentCount = 0,
                    Comments = null,
                    Contents = new List<OrgComm.Data.Models.NewsContent>()
                };
                
                if (!String.IsNullOrWhiteSpace(model.Text))
                {
                    byte[] data = null;

                    //Convert any encoding to UTF8
                    if (System.Text.Encoding.Default is System.Text.UTF8Encoding)
                    {
                        data = System.Text.Encoding.Default.GetBytes(model.Text);
                    }
                    else
                    {
                        byte[] raw = System.Text.Encoding.Default.GetBytes(model.Text);
                        data = System.Text.Encoding.Convert(Encoding.Default, Encoding.UTF8, raw);
                    }

                    news.Contents.Add(new OrgComm.Data.Models.NewsContent
                    {
                        Data = data,
                        Type = (int)OrgComm.Data.Models.NewsContent.ContentType.Text
                    });
                }

                //Temporarily
                #region Add image type
                if (model.Image1 != null)
                {
                    news.Contents.Add(new OrgComm.Data.Models.NewsContent
                    {
                        Data = model.Image1.Buffer,
                        Type = (int)OrgComm.Data.Models.NewsContent.ContentType.Image
                    });
                }

                if (model.Image2 != null)
                {
                    news.Contents.Add(new OrgComm.Data.Models.NewsContent
                    {
                        Data = model.Image2.Buffer,
                        Type = (int)OrgComm.Data.Models.NewsContent.ContentType.Image
                    });
                }

                if (model.Image3 != null)
                {
                    news.Contents.Add(new OrgComm.Data.Models.NewsContent
                    {
                        Data = model.Image3.Buffer,
                        Type = (int)OrgComm.Data.Models.NewsContent.ContentType.Image
                    });
                }

                if (model.Image4 != null)
                {
                    news.Contents.Add(new OrgComm.Data.Models.NewsContent
                    {
                        Data = model.Image4.Buffer,
                        Type = (int)OrgComm.Data.Models.NewsContent.ContentType.Image
                    });
                }

                if (model.Image5 != null)
                {
                    news.Contents.Add(new OrgComm.Data.Models.NewsContent
                    {
                        Data = model.Image5.Buffer,
                        Type = (int)OrgComm.Data.Models.NewsContent.ContentType.Image
                    });
                }
                #endregion
                //-------end temporarily

                dbc.News.Add(news);

                dbc.SaveChanges();
            }
        }

        public void SetLikeForNews(int memberId, NewsLikeRequesetModel model)
        {
            if ((model == null) || !model.NewsId.HasValue)
                throw new OrgException("Invalid news");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                OrgComm.Data.Models.News news = dbc.News.SingleOrDefault(r => r.Id == model.NewsId.Value);

                if (news == null)
                    throw new OrgException("News not found");

                var like = news.Likes.SingleOrDefault(r => r.MemberId == memberId);

                if (model.Type.HasValue)
                {
                    if (like == null)
                        news.Likes.Add(new OrgComm.Data.Models.NewsLike { MemberId = memberId, LikeType = model.Type.Value, CreatedDate = DateTime.Now });
                    else
                        like.LikeType = model.Type.Value;
                }
                else
                {
                    if (like != null)
                        dbc.Entry(like).State = System.Data.Entity.EntityState.Deleted;
                }

                news.LikeCount = news.Likes.Count();

                dbc.SaveChanges();
            }
        }

        public void CreateComment(int memberId, NewsCommentCreateRequesetModel model)
        {
            if ((model == null) || !model.NewsId.HasValue)
                throw new OrgException("Invalid news");

            if ((model == null) || !model.Type.HasValue)
                throw new OrgException("Invalid comment type");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                OrgComm.Data.Models.News news = dbc.News.SingleOrDefault(r => r.Id == model.NewsId.Value);

                if (news == null)
                    throw new OrgException("News not found");

                OrgComm.Data.Models.NewsComment comment = new OrgComm.Data.Models.NewsComment
                {
                    Text = model.Text,
                    Type = model.Type.Value,
                    MemberId = memberId,
                    CreatedDate = DateTime.Now
                };

                news.Comments.Add(comment);
                news.CommentCount = news.Comments.Count();

                dbc.SaveChanges();
            }
        }

        public byte[] GetImageNewsContentById(int newsContentId)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                byte[] image = dbc.NewsContent.Where(r => (r.Type == (int)OrgComm.Data.Models.NewsContent.ContentType.Image) && (r.Id == newsContentId)).Select(r => r.Data).FirstOrDefault();

                if (image == null)
                    throw new OrgException("Content not found");

                return image;
            }
        }

        public IList<NewsModel> GetNewsByMember(int memberId, NewsByCriteriaRequestModel model)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var member = dbc.Members.SingleOrDefault(r => r.Id == memberId);

                if (member == null)
                    throw new OrgException("Invalid member");

                var qry = (from n in dbc.News
                           where n.CompanyId == member.CompanyId
                           select n);
                
                qry = qry.OrderByDescending(r => r.CreatedDate);

                if (model != null)
                {
                    if (model.PageSize.HasValue && (model.PageSize.Value > 0))
                    {
                        if (model.Page.HasValue && (model.Page.Value > 1))
                            qry = qry.Skip((model.Page.Value -1) * model.PageSize.Value);

                        qry = qry.Take(model.PageSize.Value);
                    }
                }

                var news = qry.ToList();
                
                return news.Select(r => new NewsModel
                {
                    Id = r.Id,
                    LikeCount = r.LikeCount,
                    CommentCount = r.CommentCount,
                    Likes = r.Likes.Select(rr => new NewsLikeModel
                    {
                        MemberId = rr.MemberId,
                        LikeType = rr.LikeType
                    }).ToList(),
                    Text = r.Contents.Where(cr => cr.Type == (int)OrgComm.Data.Models.NewsContent.ContentType.Text).Select(cr => Encoding.Default.GetString(Encoding.Convert(Encoding.UTF8, Encoding.Default, cr.Data))).ToList(),
                    ImageUrl = r.Contents.Where(cr => cr.Type == (int)OrgComm.Data.Models.NewsContent.ContentType.Image).Select(cr => String.Format(NewsBL.ImageUrlFormatString, cr.Id)).ToList(),
                    CreatedDate = r.CreatedDate.ToString(AppConfigs.GeneralDateTimeFormat),
                    Ticks = r.CreatedDate.Ticks
                }).ToList();
            }
        }

        public IList<NewsCommentModel> GetCommentsByNewsId(NewsCommentByCriteriaRequestModel model)
        {
            if ((model == null) || !model.NewsId.HasValue)
                throw new OrgException("Invalid news");
            
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                OrgComm.Data.Models.News news = dbc.News.SingleOrDefault(r => r.Id == model.NewsId.Value);

                if (news == null)
                    throw new OrgException("News not found");
                
                var qry = (from c in news.Comments
                           join m in dbc.Members on c.MemberId equals m.Id
                           select new OrgComm.Data.Models.NewsComment
                           {
                               Id = c.Id,
                               MemberId = (m.DelFlag)? 0 : c.MemberId,
                               NewsId = c.NewsId,
                               Text = c.Text,
                               Type = c.Type,
                               CreatedDate = c.CreatedDate
                           }
                          );

                qry = qry.OrderByDescending(r => r.CreatedDate);

                if (model != null)
                {
                    if (model.PageSize.HasValue && (model.PageSize.Value > 0))
                    {
                        if (model.Page.HasValue && (model.Page.Value > 1))
                            qry = qry.Skip((model.Page.Value - 1) * model.PageSize.Value);

                        qry = qry.Take(model.PageSize.Value);
                    }
                }

                return qry.Select(r => new NewsCommentModel
                {
                    Id = r.Id,
                    MemberId = r.MemberId,
                    Text = r.Text,
                    Type = r.Type,
                    CreatedDate = r.CreatedDate.ToString(AppConfigs.GeneralDateTimeFormat),
                    Ticks = r.CreatedDate.Ticks
                }).ToList();
            }
        }
    }
}