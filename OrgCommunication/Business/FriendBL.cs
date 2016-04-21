using System.Linq;
using OrgCommunication.Models.Friend;
using System.Collections.Generic;
using OrgComm.Data;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using System;
using System.Web;

namespace OrgCommunication.Business
{
    public class FriendBL
    {
        public FriendBL()
        {

        }

        public void SetFavouriteFriend(int memberId, FriendFavouriteRequestModel model)
        {
            if (!model.FriendMemberId.HasValue)
                throw new OrgException(1, "Invalid friend Id");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                OrgComm.Data.Models.Member member = dbc.Members.FirstOrDefault(r => r.Id.Equals(memberId));

                if (member == null)
                    throw new OrgException(1, "Invalid profile");

                var friend = dbc.Friends.SingleOrDefault(r => r.MemberId.Equals(member.Id) && r.FriendMemberId.Equals(model.FriendMemberId.Value));

                if (friend == null) //Not in friend list
                {
                    if (!dbc.Members.Any(r => r.Id.Equals(model.FriendMemberId.Value)))
                        throw new OrgException(1, "Invalid profile");

                    dbc.Friends.Add(new OrgComm.Data.Models.Friend
                    {
                        MemberId = member.Id,
                        FriendMemberId = model.FriendMemberId.Value,
                        Status = (int)OrgComm.Data.Models.Friend.StatusType.Active,
                        IsFavourite = model.IsFavourite,
                        AddedDate = DateTime.Now,
                        UpdatedDate = null

                    });
                }
                else
                {
                    friend.IsFavourite = model.IsFavourite;
                }

                dbc.SaveChanges();
            }
        }

        public void SetFriendStatus(int memberId, int friendMemberId, OrgComm.Data.Models.Friend.StatusType type)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                OrgComm.Data.Models.Member member = dbc.Members.FirstOrDefault(r => r.Id.Equals(memberId) && (!r.DelFlag));

                if (member == null)
                    throw new OrgException(1, "Invalid profile");

                if (!dbc.Members.Any(r => r.Id.Equals(friendMemberId) && (!r.DelFlag)))
                    throw new OrgException(1, "Invalid friend profile");

                var friend = dbc.Friends.SingleOrDefault(r => r.MemberId.Equals(member.Id) && r.FriendMemberId.Equals(friendMemberId));

                if (friend == null) //Not in friend list
                {
                    dbc.Friends.Add(new OrgComm.Data.Models.Friend
                    {
                        MemberId = member.Id,
                        FriendMemberId = friendMemberId,
                        Status = (int)type,
                        IsFavourite = false,
                        AddedDate = DateTime.Now,
                        UpdatedDate = null

                    });
                }
                else
                {
                    friend.Status = (int)type;
                    friend.UpdatedDate = DateTime.Now;
                }

                dbc.SaveChanges();
            }
        }

        public IList<FriendMemberModel> GetFriends(int memberId, bool? isFavourite, OrgComm.Data.Models.Friend.StatusType? type)
        {
            List<FriendMemberModel> friendList = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                OrgComm.Data.Models.Member member = dbc.Members.SingleOrDefault(r => r.Id.Equals(memberId));

                if (member == null)
                    throw new OrgException(1, "Invalid profile");

                var lookup = dbc.Lookups.SingleOrDefault(r => (r.TypeId == (int)OrgComm.Data.Models.Lookup.LookupType.FriendStatus) && (r.Value == (int)OrgComm.Data.Models.Friend.StatusType.Active));
                string friendStatusDesc = String.Empty;

                if (lookup != null)
                    friendStatusDesc = lookup.Description;

                var qry = from m in dbc.Members
                          join f in dbc.Friends on m.Id equals f.FriendMemberId into fm
                          from mwithf in fm.DefaultIfEmpty()
                          join l in dbc.Lookups on new { type = (int)OrgComm.Data.Models.Lookup.LookupType.FriendStatus, status = ((mwithf == null) ? (int)OrgComm.Data.Models.Friend.StatusType.Active : mwithf.Status) } equals new { type = l.TypeId, status = l.Value }
                          where m.CompanyId == member.CompanyId // friend must be in same company
                                && m.Id != member.Id // not request member
                                && m.DelFlag == false // not delete account
                                && ((type == null) || (((mwithf == null) ? (int)OrgComm.Data.Models.Friend.StatusType.Active : mwithf.Status) == (int)type.Value))
                                && ((isFavourite == null) || (((mwithf == null) ? false : mwithf.IsFavourite) == isFavourite.Value))
                          orderby m.Id
                          select new FriendMemberModel
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
                              Photo = (m.Photo == null) ? null : m.Id.ToString(),
                              Status = (mwithf == null) ? (int)OrgComm.Data.Models.Friend.StatusType.Active : mwithf.Status,
                              StatusDescription = l.Description
                          };
                
                friendList = qry.ToList();
                
                string templateUrl = MemberBL.PhotoUrlFormatString;

                friendList.ForEach(r =>
                {
                    if (r.Photo != null)
                        r.Photo = string.Format(templateUrl, r.Id);

                    r.RoomId = ChatBL.GetChatRoomId(new int[] { memberId, r.Id }, ChatBL.ParticipationType.Member);
                });
            }

            return friendList;
        }

        public void RemoveFriendByMemberId(int memberId)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var friends = dbc.Friends.Where(r => r.MemberId.Equals(memberId) || r.FriendMemberId.Equals(memberId));
                if (friends.Count() > 0)
                    dbc.Friends.RemoveRange(friends);

                dbc.SaveChanges();
            }
        }

    }
}