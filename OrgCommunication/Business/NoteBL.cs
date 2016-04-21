using System.Linq;
using OrgCommunication.Models.Note;
using System.Collections.Generic;
using OrgComm.Data;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using System;
using System.Web;

namespace OrgCommunication.Business
{
    public class NoteBL
    {
        private static string _imageUrlFormatString = null;
        public static string ImageUrlFormatString
        {
            get
            {
                string url = null;

                if (System.Web.HttpContext.Current == null)
                {
                    return "/Note/Image?Id={0}";
                }
                else
                {
                    if (_imageUrlFormatString == null)
                        _imageUrlFormatString = string.Format("{0}{1}{2}",
                        HttpContext.Current.Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped),
                        (HttpContext.Current.Request.ApplicationPath.Equals("/")) ? "/" : HttpContext.Current.Request.ApplicationPath
                        , "Note/Image?Id={0}");

                    return _imageUrlFormatString;
                }
            }
        }

        public NoteBL()
        {

        }

        public NoteModel CreateNoteMessage(int memberId, NoteMessageCreateRequestModel model)
        {
            if ((model == null) || String.IsNullOrWhiteSpace(model.Text))
                throw new OrgException("Invalid note");

            return this.CreateNote(new OrgComm.Data.Models.Note
            {
                MemberId = memberId,
                Text = model.Text,
                Type = (int)OrgComm.Data.Models.Note.NoteType.Text,
                CreatedDate = DateTime.Now
            });
        }

        public NoteModel CreateNoteImage(int memberId, NoteImageCreateRequestModel model)
        {
            if ((model == null) || (model.Image == null))
                throw new OrgException("Invalid note");

            return this.CreateNote(new OrgComm.Data.Models.Note
            {
                MemberId = memberId,
                Image = model.Image.Buffer,
                Type = (int)OrgComm.Data.Models.Note.NoteType.Image,
                CreatedDate = DateTime.Now
            });
        }

        private NoteModel CreateNote(OrgComm.Data.Models.Note note)
        {
            NoteModel noteModel = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                if (!dbc.Members.Any(r => (!r.DelFlag) && (r.Id.Equals(note.MemberId))))
                    throw new OrgException("Invalid member");

                dbc.Notes.Add(note);
                dbc.SaveChanges();

                noteModel = new NoteModel
                {
                    Id = note.Id,
                    Text = note.Text,
                    Image = (note.Image == null) ? null : NewsBL.ImageUrlFormatString.Replace("{0}", note.Id.ToString()),
                    Type = note.Type,
                    CreatedDate = note.CreatedDate.ToString(AppConfigs.GeneralDateTimeFormat),
                    Ticks = note.CreatedDate.Ticks
                };

                var lookupNoteType = dbc.Lookups.SingleOrDefault(r => (r.TypeId == (int)OrgComm.Data.Models.Lookup.LookupType.NoteType) && (r.Value == (int)note.Type));

                if (lookupNoteType != null)
                    noteModel.TypeDescription = lookupNoteType.Description;
            }

            return noteModel;
        }

        public NoteModel UpdateNoteMessage(int? memberId, NoteMessageUpdateRequestModel model)
        {
            if ((model == null) || !model.Id.HasValue || String.IsNullOrWhiteSpace(model.Text))
                throw new OrgException("Invalid note");

            return this.UpdateNote(memberId, new OrgComm.Data.Models.Note
            {
                Id = model.Id.Value,
                Text = model.Text,
                Type = (int)OrgComm.Data.Models.Note.NoteType.Text
            });
        }

        public NoteModel UpdateNoteImage(int? memberId, NoteImageUpdateRequestModel model)
        {
            if ((model == null) || !model.Id.HasValue || (model.Image == null))
                throw new OrgException("Invalid note");

            return this.UpdateNote(memberId, new OrgComm.Data.Models.Note
            {
                Id = model.Id.Value,
                Image = model.Image.Buffer,
                Type = (int)OrgComm.Data.Models.Note.NoteType.Image
            });
        }

        private NoteModel UpdateNote(int? memberId, OrgComm.Data.Models.Note updateNote)
        {
            NoteModel noteModel = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var note = dbc.Notes.SingleOrDefault(r => r.Id.Equals(updateNote.Id));

                if (note == null)
                    throw new OrgException("Invalid note");

                if (memberId.HasValue && !note.MemberId.Equals(memberId.Value))
                    throw new OrgException("Invalid note owner");

                if (updateNote.Type == (int)OrgComm.Data.Models.Note.NoteType.Text)
                {
                    note.Type = updateNote.Type;
                    note.Text = updateNote.Text;
                    note.Image = null;
                }
                else if (updateNote.Type == (int)OrgComm.Data.Models.Note.NoteType.Image)
                {
                    note.Type = updateNote.Type;
                    note.Text = null;
                    note.Image = updateNote.Image;
                }

                dbc.SaveChanges();

                noteModel = new NoteModel
                {
                    Id = note.Id,
                    Text = note.Text,
                    Image = (note.Image == null) ? null : NewsBL.ImageUrlFormatString.Replace("{0}", note.Id.ToString()),
                    Type = note.Type,
                    CreatedDate = note.CreatedDate.ToString(AppConfigs.GeneralDateTimeFormat),
                    Ticks = note.CreatedDate.Ticks
                };

                var lookupNoteType = dbc.Lookups.SingleOrDefault(r => (r.TypeId == (int)OrgComm.Data.Models.Lookup.LookupType.NoteType) && (r.Value == (int)note.Type));

                if (lookupNoteType != null)
                    noteModel.TypeDescription = lookupNoteType.Description;
            }

            return noteModel;
        }

        public void RemoveNoteById(int? memberId, int noteId)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var note = dbc.Notes.SingleOrDefault(r => r.Id.Equals(noteId));

                if (note == null)
                    throw new OrgException("Invalid note");

                if (memberId.HasValue && !note.MemberId.Equals(memberId.Value))
                    throw new OrgException("Invalid note owner");

                dbc.Notes.Remove(note);
                dbc.SaveChanges();
            }
        }

        public void RemoveNoteByMemberId(int memberId)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var noteList = dbc.Notes.Where(r => r.MemberId.Equals(memberId));

                if (noteList.Count() > 0)
                {
                    dbc.Notes.RemoveRange(noteList);
                    dbc.SaveChanges();
                }
            }
        }

        public byte[] GetNoteImage(NoteRequestModel model)
        {
            byte[] byteImage = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var qry = dbc.Notes.AsQueryable();
                byte[] image = null;

                if (model.Id.HasValue)
                {
                    image = qry.Where(r => (r.Id == model.Id.Value)).Select(r => r.Image).FirstOrDefault();
                }
                else
                {
                    throw new OrgException("Invalid id");
                }

                if (image == null)
                    throw new OrgException("Image not found");
                else
                    return image;
            }
        }

        public NoteModel GetNotesById(int noteId)
        {
            NoteModel note = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var temp = (from n in dbc.Notes
                        join l in dbc.Lookups on new { type = (int)OrgComm.Data.Models.Lookup.LookupType.NoteType, noteType = n.Type } equals new { type = l.TypeId, noteType = l.Value } into nl
                        from nex in nl.DefaultIfEmpty()
                        where n.Id.Equals(noteId)
                        orderby n.Id
                        select new
                        {
                            Id = n.Id,
                            Type = n.Type,
                            TypeDescription = (nex == null) ? null : nex.Description,
                            Text = n.Text,
                            Image = (n.Image == null) ? null : NoteBL.ImageUrlFormatString.Replace("{0}", n.Id.ToString()),
                            CreatedDate = n.CreatedDate
                        }).FirstOrDefault();

                if (temp != null)
                    note = new NoteModel
                    {
                        Id = temp.Id,
                        Type = temp.Type,
                        TypeDescription = temp.TypeDescription,
                        Text = temp.Text,
                        Image = temp.Image,
                        CreatedDate = temp.CreatedDate.ToString(AppConfigs.GeneralDateTimeFormat),
                        Ticks = temp.CreatedDate.Ticks
                    };
            }

            return note;
        }

        public IList<NoteModel> GetNotesByMember(int memberId)
        {
            List<NoteModel> noteList = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var tempList = (from n in dbc.Notes
                            join l in dbc.Lookups on new { type = (int)OrgComm.Data.Models.Lookup.LookupType.NoteType, noteType = n.Type } equals new { type = l.TypeId, noteType = l.Value } into nl
                            from nex in nl.DefaultIfEmpty()
                            where n.MemberId.Equals(memberId)
                            orderby n.Id
                            select new
                            {
                                Id = n.Id,
                                Type = n.Type,
                                TypeDescription = (nex == null) ? null : nex.Description,
                                Text = n.Text,
                                Image = (n.Image == null) ? null : NoteBL.ImageUrlFormatString.Replace("{0}", n.Id.ToString()),
                                CreatedDate = n.CreatedDate
                            }).ToList();

                noteList = tempList.Select(r => new NoteModel
                {
                    Id = r.Id,
                    Type = r.Type,
                    TypeDescription = r.TypeDescription,
                    Text = r.Text,
                    Image = r.Image,
                    CreatedDate = r.CreatedDate.ToString(AppConfigs.GeneralDateTimeFormat),
                    Ticks = r.CreatedDate.Ticks
                }).ToList();
            }

            return noteList;
        }
    }
}