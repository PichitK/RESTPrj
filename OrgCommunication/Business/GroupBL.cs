using System.Linq;
using OrgCommunication.Models.Group;
using System.Collections.Generic;
using OrgComm.Data;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using System;
using OrgCommunication.Helpers.Drawing;
using System.Drawing;
using System.Web;

namespace OrgCommunication.Business
{
    public class GroupBL
    {
        private static string _logoUrlFormatString = null;
        public static string LogoUrlFormatString
        {
            get
            {
                string url = null;

                if (System.Web.HttpContext.Current == null)
                {
                    return "/Group/Logo?Id={0}";
                }
                else
                {
                    if (_logoUrlFormatString == null)
                        _logoUrlFormatString = string.Format("{0}{1}{2}",
                        HttpContext.Current.Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped),
                        (HttpContext.Current.Request.ApplicationPath.Equals("/")) ? "/" : HttpContext.Current.Request.ApplicationPath
                        , "Group/Logo?Id={0}");

                    return _logoUrlFormatString;
                }
            }
        }

        public GroupBL()
        {

        }

        public GroupModel CreateGroup(int memberId, CreateGroupRequestModel model, OrgComm.Data.Models.Group.GroupType groupType)
        {
            GroupModel groupModel = null;

            OrgComm.Data.Models.Group group = new OrgComm.Data.Models.Group
            {
                FounderId = memberId,
                Type = (int)groupType,
                Title = model.Title,
                SubTitle = model.SubTitle,
                WelcomeMessage = model.WelcomeMessage,
                CreatedDate = DateTime.Now
            };

            if (model.Logo != null)
            {
                byte[] logo = model.Logo.Buffer;

                using (System.IO.MemoryStream msReader = new System.IO.MemoryStream(logo))
                {
                    using (System.Drawing.Image img = System.Drawing.Image.FromStream(msReader))
                    {
                        string imageType = ImageHelper.GetImageFormat(img);

                        if (imageType == null)
                            throw new OrgException("Not support image type");

                        int? width, height;
                        Image imgResize = null;

                        //Size Max constraint
                        width = AppConfigs.GroupPhotoWidthMax;
                        height = AppConfigs.GroupPhotoHeightMax;

                        imgResize = ImageHelper.ReSize(img, width, height, ImageHelper.ResizeMode.KeepAspectRatio);
                        group.Logo = ImageHelper.ImageToByteArray(imgResize, img.RawFormat);
                    }
                }
            }

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                if (groupType == OrgComm.Data.Models.Group.GroupType.Other)
                {
                    if (!dbc.Members.Any(r => (r.Id.Equals(group.FounderId))))
                        throw new OrgException("Invalid member");
                }
                else if (groupType == OrgComm.Data.Models.Group.GroupType.Company)
                {
                    if (!dbc.Company.Any(r => (r.Id.Equals(group.FounderId))))
                        throw new OrgException("Invalid Company");
                }

                dbc.Groups.Add(group);
                dbc.SaveChanges();

                var lookupGroupType = dbc.Lookups.SingleOrDefault(r => (r.TypeId == (int)OrgComm.Data.Models.Lookup.LookupType.GroupType) && (r.Value == (int)groupType));
                var lookupJoinedStatus = dbc.Lookups.SingleOrDefault(r => (r.TypeId == (int)OrgComm.Data.Models.Lookup.LookupType.GroupMemberJoinedStatus) && (r.Value == (int)OrgComm.Data.Models.GroupMember.JoinedStatusType.Active));

                groupModel = new GroupModel
                {
                    Id = group.Id,
                    FounderId = group.FounderId,
                    Type = group.Type,
                    TypeDescription = (lookupGroupType == null) ? null : lookupGroupType.Description,
                    Title = group.Title,
                    SubTitle = group.SubTitle,
                    WelcomeMessage = group.WelcomeMessage,
                    Logo = (group.Logo == null) ? null : String.Format(GroupBL.LogoUrlFormatString, group.Id),
                    Status = (int)OrgComm.Data.Models.GroupMember.JoinedStatusType.Active,
                    StatusDescription = (lookupJoinedStatus == null) ? null : lookupJoinedStatus.Description,
                    RoomId = ChatBL.GetChatRoomId(new int[] { group.Id }, ChatBL.ParticipationType.Group),
                };

                if (groupType == OrgComm.Data.Models.Group.GroupType.Other)
                {
                    try
                    {
                        var groupmember = new OrgComm.Data.Models.GroupMember
                        {
                            GroupId = group.Id,
                            MemberId = group.FounderId,
                            JoinedDate = group.CreatedDate,
                            JoinedStatus = (int)OrgComm.Data.Models.GroupMember.JoinedStatusType.Active
                        };

                        dbc.GroupMembers.Add(groupmember);

                        dbc.SaveChanges();
                    }
                    catch (System.Exception ex)
                    {
                        throw new OrgException("Cannot member to group", ex);
                    }

                    groupModel.Members = GroupBL.GetGroupMember(dbc, group.Id);
                }
            }

            return groupModel;
        }

        public void UpdateGroup(int memberId, UpdateGroupRequestModel model)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var group = dbc.Groups.SingleOrDefault(r => r.Id.Equals(model.GroupId.Value));

                if (group == null)
                    throw new OrgException("Group not found");

                if ((group.Type == (int)OrgComm.Data.Models.Group.GroupType.Company)
                    || (!dbc.GroupMembers.Any(r => r.GroupId.Equals(group.Id) && r.MemberId.Equals(memberId))))
                    throw new OrgException("Not authorized to update group");

                if (!String.IsNullOrWhiteSpace(model.Title))
                    group.Title = model.Title;

                if (!String.IsNullOrWhiteSpace(model.SubTitle))
                    group.SubTitle = model.SubTitle;

                if (!String.IsNullOrWhiteSpace(model.WelcomeMessage))
                    group.WelcomeMessage = model.WelcomeMessage;

                if (model.Logo != null)
                {
                    byte[] logo = model.Logo.Buffer;

                    using (System.IO.MemoryStream msReader = new System.IO.MemoryStream(logo))
                    {
                        using (System.Drawing.Image img = System.Drawing.Image.FromStream(msReader))
                        {
                            string imageType = ImageHelper.GetImageFormat(img);

                            if (imageType == null)
                                throw new OrgException("Not support image type");

                            int? width, height;
                            Image imgResize = null;

                            //Size Max constraint
                            width = AppConfigs.GroupPhotoWidthMax;
                            height = AppConfigs.GroupPhotoHeightMax;

                            imgResize = ImageHelper.ReSize(img, width, height, ImageHelper.ResizeMode.KeepAspectRatio);
                            group.Logo = ImageHelper.ImageToByteArray(imgResize, img.RawFormat);
                        }
                    }
                }

                dbc.SaveChanges();
            }
        }

        public void AddMember(int inviterId, AddGroupMemberRequestModel model)
        {
            if (!model.GroupId.HasValue)
                throw new OrgException("Invalid group Id");

            if (!model.MemberId.HasValue)
                throw new OrgException("Invalid member Id");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                if (!dbc.Members.Any(r => (r.Id.Equals(model.MemberId.Value))))
                    throw new OrgException("Member not found");

                if (!dbc.Groups.Any(r => (r.Id.Equals(model.GroupId.Value))))
                    throw new OrgException("Group not found");

                if (dbc.GroupMembers.Any(r => r.GroupId.Equals(model.GroupId.Value) && r.MemberId.Equals(model.MemberId.Value)))
                    throw new OrgException("Already in group");

                try
                {
                    dbc.GroupMembers.Add(new OrgComm.Data.Models.GroupMember
                    {
                        GroupId = model.GroupId.Value,
                        InviterMemberId = inviterId,
                        MemberId = model.MemberId.Value,
                        JoinedDate = DateTime.Now,
                        JoinedStatus = (int)OrgComm.Data.Models.GroupMember.JoinedStatusType.Invited
                    });

                    dbc.SaveChanges();
                }
                catch (System.Exception ex)
                {
                    throw new OrgException("Cannot member to group", ex);
                }
            }
        }

        public void AcceptInvitaion(int memberId, AcceptGroupInvitationRequestModel model)
        {
            if (!model.GroupId.HasValue)
                throw new OrgException("Invalid group Id");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                if (!dbc.Members.Any(r => (r.Id.Equals(memberId))))
                    throw new OrgException("Member not found");

                if (!dbc.Groups.Any(r => (r.Id.Equals(model.GroupId.Value))))
                    throw new OrgException("Group not found");

                var groupmember = dbc.GroupMembers.SingleOrDefault(r => r.GroupId.Equals(model.GroupId.Value) && r.MemberId.Equals(memberId));

                if (groupmember == null)
                {
                    throw new OrgException(1, "No group invitation");
                }
                else
                {
                    switch (groupmember.JoinedStatus)
                    {
                        case (int)OrgComm.Data.Models.GroupMember.JoinedStatusType.Active: throw new OrgException("Already in group");
                        case (int)OrgComm.Data.Models.GroupMember.JoinedStatusType.Block: throw new OrgException(2, "No group invitation");
                        case (int)OrgComm.Data.Models.GroupMember.JoinedStatusType.Requested: throw new OrgException(3, "No group invitation");
                        case (int)OrgComm.Data.Models.GroupMember.JoinedStatusType.Suspend: throw new OrgException(4, "No group invitation");
                        default: break; //(int)OrgComm.Data.Models.GroupMember.JoinedStatusType.Invited
                    }
                }

                try
                {
                    groupmember.JoinedStatus = (int)OrgComm.Data.Models.GroupMember.JoinedStatusType.Active;
                    groupmember.JoinedDate = DateTime.Now;

                    dbc.SaveChanges();
                }
                catch (System.Exception ex)
                {
                    throw new OrgException("Cannot member to group", ex);
                }
            }
        }

        public void Leave(int memberId, LeaveGroupRequestModel model)
        {
            if (!model.GroupId.HasValue)
                throw new OrgException("Invalid group Id");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var group = dbc.Groups.SingleOrDefault(r => r.Id.Equals(model.GroupId.Value));

                if (group == null)
                    throw new OrgException("Group not found");

                var groupmeber = dbc.GroupMembers.SingleOrDefault(r => r.GroupId.Equals(model.GroupId.Value) && r.MemberId.Equals(memberId));

                if (groupmeber == null)
                    throw new OrgException("Member not in group");

                try
                {
                    dbc.GroupMembers.Remove(groupmeber);

                    dbc.SaveChanges();
                }
                catch (System.Exception ex)
                {
                    throw new OrgException("Cannot leave group", ex);
                }

                if (group.Type == (int)OrgComm.Data.Models.Group.GroupType.Other)
                {
                    if (dbc.GroupMembers.Where(r => r.GroupId.Equals(model.GroupId.Value)).Count() == 0)
                    {
                        dbc.Groups.Remove(group);

                        dbc.SaveChanges();
                    }
                }
            }
        }

        public byte[] GetGroupLogo(GroupRequestModel model)
        {
            byte[] byteImage = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var qry = dbc.Groups.AsQueryable();
                byte[] logo = null;

                if (model.Id.HasValue)
                {
                    logo = qry.Where(r => (r.Id == model.Id.Value)).Select(r => r.Logo).FirstOrDefault();
                }
                else
                {
                    throw new OrgException("Invalid id");
                }

                if (logo == null)
                    throw new OrgException("Logo not found");
                else
                    return logo;
            }
        }

        public IList<GroupModel> GetGroupsByMember(int memberId, OrgComm.Data.Models.GroupMember.JoinedStatusType? joinedType)
        {
            List<GroupModel> groupList = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                groupList = (from g in dbc.Groups
                             join gm in dbc.GroupMembers on g.Id equals gm.GroupId
                             join l in dbc.Lookups on new { type = (int)OrgComm.Data.Models.Lookup.LookupType.GroupType, groupType = g.Type } equals new { type = l.TypeId, groupType = l.Value } into gl
                             from gex in gl.DefaultIfEmpty()
                             join l2 in dbc.Lookups on new { type = (int)OrgComm.Data.Models.Lookup.LookupType.GroupMemberJoinedStatus, joinedType = gm.JoinedStatus } equals new { type = l2.TypeId, joinedType = l2.Value } into gml
                             from gmex in gml.DefaultIfEmpty()
                             where gm.MemberId.Equals(memberId) && ((joinedType == null) || gm.JoinedStatus.Equals((int)joinedType.Value))
                             orderby g.Id
                             select new GroupModel
                             {
                                 Id = g.Id,
                                 Type = g.Type,
                                 TypeDescription = (gex == null) ? null : gex.Description,
                                 FounderId = g.FounderId,
                                 Title = g.Title,
                                 SubTitle = g.SubTitle,
                                 WelcomeMessage = g.WelcomeMessage,
                                 Logo = (g.Logo == null) ? null : g.Id.ToString(),
                                 Status = gm.JoinedStatus,
                                 StatusDescription = (gmex == null) ? null : gmex.Description
                             }).ToList();

                groupList.ForEach(r =>
                {
                    if (r.Logo != null)
                        r.Logo = String.Format(GroupBL.LogoUrlFormatString, r.Id);

                    r.RoomId = ChatBL.GetChatRoomId(new int[] { r.Id }, ChatBL.ParticipationType.Group);
                    r.Members = GroupBL.GetGroupMember(dbc, r.Id);
                });
            }

            return groupList;
        }

        public void LeaveAllGroupsByMemberId(int memberId)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var groupmembers = dbc.GroupMembers.Where(r => r.MemberId.Equals(memberId));
                if (groupmembers.Count() > 0)
                    dbc.GroupMembers.RemoveRange(groupmembers);

                dbc.SaveChanges();

                var groups = dbc.Groups.Where(r => (r.Type == (int)OrgComm.Data.Models.Group.GroupType.Other) && !dbc.GroupMembers.Any(rr => rr.GroupId.Equals(r.Id)));
                if (groups.Count() > 0)
                    dbc.Groups.RemoveRange(groups);

                dbc.SaveChanges();
            }
        }

        private static IList<Models.Member.MemberModel> GetGroupMember(OrgCommEntities context, int groupId)
        {
            var qry = from gm in context.GroupMembers
                      join l in context.Lookups on new { type = (int)OrgComm.Data.Models.Lookup.LookupType.GroupMemberJoinedStatus, value = gm.JoinedStatus } equals new { type = l.TypeId, value = l.Value }
                      join m in context.Members on gm.MemberId equals m.Id
                      where gm.GroupId == groupId
                      orderby m.Id
                      select new Models.Member.MemberModel
                      {
                          Id = m.Id,
                          FacebookId = m.FacebookId,
                          Email = m.Email,
                          FirstName = m.FirstName,
                          LastName = m.LastName,
                          NickName = m.Nickname,
                          DisplayName = m.DisplayName,
                          Gender = m.Gender,
                          Company = m.Company.Name,
                          Department = m.Department.Name,
                          Position = m.Position.Name,
                          EmployeeId = m.EmployeeId,
                          Phone = m.Phone,
                          Photo = MemberBL.PhotoUrlFormatString.Replace("{0}", m.Id.ToString())
                      };

            return qry.ToList();
        }
    }
}