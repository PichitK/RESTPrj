using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers.Security;
using OrgCommunication.Models;
using OrgCommunication.Models.Friend;
using OrgCommunication.Models.Group;
using System;
using System.Linq;
using System.Web.Http;

namespace OrgCommunication.APIs
{
    /// <summary>
    /// Contact API
    /// </summary>
    public class ContactController : ApiController
    {
        /// <summary>
        /// Add/Remove friend in favourite list
        /// </summary>
        /// <param name="param">Set favourite request model </param>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [Authorize]
        public ResultModel SetFavourite(FriendFavouriteRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                FriendBL bl = new FriendBL();

                bl.SetFavouriteFriend(memberId.Value, param);

                result.Status = true;
                result.Message = "Update favorite friend successfully";
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
        /// Block/Un-block friend
        /// </summary>
        /// <param name="param">Set block/un-block request model </param>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [Authorize]
        public ResultModel SetBlock(FriendBlockRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                if (!param.FriendMemberId.HasValue)
                    throw new OrgException("Invalid firend MemberId");

                FriendBL bl = new FriendBL();

                bl.SetFriendStatus(memberId.Value, param.FriendMemberId.Value, (param.IsBlocked)? OrgComm.Data.Models.Friend.StatusType.Block : OrgComm.Data.Models.Friend.StatusType.Active);

                result.Status = true;
                result.Message = "Update friend status successfully";
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
        /// Hide/Un-hide friend
        /// </summary>
        /// <param name="param">Hide/un-hide request model </param>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [Authorize]
        public ResultModel SetHidden(FriendHiddenRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                if (!param.FriendMemberId.HasValue)
                    throw new OrgException("Invalid firend MemberId");

                FriendBL bl = new FriendBL();

                bl.SetFriendStatus(memberId.Value, param.FriendMemberId.Value, (param.IsHidden) ? OrgComm.Data.Models.Friend.StatusType.Hide : OrgComm.Data.Models.Friend.StatusType.Active);

                result.Status = true;
                result.Message = "Update friend status successfully";
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
        /// Get blockde friends list
        /// </summary>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [Authorize]
        [HttpPost]
        public FriendResultModel GetBlockedMember()
        {
            FriendResultModel result = new FriendResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid member MemberId");

                FriendBL friendBl = new FriendBL();
                
                var friendList = friendBl.GetFriends(memberId.Value, null, OrgComm.Data.Models.Friend.StatusType.Block);
                
                result.Status = true;
                result.Message = ((friendList.Count == 0) ? "No blocked-friend found." : friendList.Count.ToString() + " blocked-friends found. ");
                result.Friends = friendList;
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
        /// Get hidden friends list
        /// </summary>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [Authorize]
        [HttpPost]
        public FriendResultModel GetHiddenMember()
        {
            FriendResultModel result = new FriendResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid member MemberId");

                FriendBL friendBl = new FriendBL();

                var friendList = friendBl.GetFriends(memberId.Value, null, OrgComm.Data.Models.Friend.StatusType.Hide);
                
                result.Status = true;
                result.Message = ((friendList.Count == 0) ? "No hidden-friend found." : friendList.Count.ToString() + " hiddend-friends found. ");
                result.Friends = friendList;
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
        /// Get friends and group list
        /// </summary>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [Authorize]
        [HttpPost]
        public GroupAndFriendResultModel GetContactList()
        {
            GroupAndFriendResultModel result = new GroupAndFriendResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid member MemberId");

                FriendBL friendBl = new FriendBL();

                var favouriteList = friendBl.GetFriends(memberId.Value, true, OrgComm.Data.Models.Friend.StatusType.Active);
                var friendList = friendBl.GetFriends(memberId.Value, false, OrgComm.Data.Models.Friend.StatusType.Active);

                GroupBL groupBl = new GroupBL();

                var groupList = groupBl.GetGroupsByMember(memberId.Value, null);

                result.Status = true;
                result.Message = ((friendList.Count + favouriteList.Count == 0) ? "No friend found." : (friendList.Count + favouriteList.Count).ToString() + " friends found. ")
                                + ((groupList.Count == 0) ? "No group found." : groupList.Count.ToString() + " groups found.");

                result.Favourites = favouriteList;
                result.Friends = friendList;
                result.Groups = groupList;
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
        /// Create new group
        /// </summary>
        /// <param name="param">Add Request Model</param>
        /// <remarks></remarks>
        [SwaggerConfig.SwashConsumeMultipart(typeof(CreateGroupRequestModel))]
        [Authorize]
        public GroupResultModel CreateGroup(CreateGroupRequestModel param)
        {
            GroupResultModel result = new GroupResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                GroupBL bl = new GroupBL();

                var group = bl.CreateGroup(memberId.Value, param, OrgComm.Data.Models.Group.GroupType.Other);

                result.Status = true;
                result.Message = "Group created";
                result.Group = group;
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
        /// Update group info
        /// </summary>
        /// <param name="param">Update Request Model</param>
        /// <remarks></remarks>
        [SwaggerConfig.SwashConsumeMultipart(typeof(UpdateGroupRequestModel))]
        [Authorize]
        public ResultModel UpdateGroup(UpdateGroupRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                GroupBL bl = new GroupBL();

                bl.UpdateGroup(memberId.Value, param);

                result.Status = true;
                result.Message = "Group updated";
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
        /// Add group member
        /// </summary>
        /// <param name="param">Add Member Request Model</param>
        /// <remarks></remarks>
        [Authorize]
        public ResultModel AddGroupMember(AddGroupMemberRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                GroupBL bl = new GroupBL();

                bl.AddMember(memberId.Value, param);

                result.Status = true;
                result.Message = "Invited member";
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
        /// Accept group invitation
        /// </summary>
        /// <param name="param">Accept Invitation Request Model</param>
        /// <remarks></remarks>
        [Authorize]
        public ResultModel AcceptGroupInvitation(AcceptGroupInvitationRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                GroupBL bl = new GroupBL();

                bl.AcceptInvitaion(memberId.Value, param);

                result.Status = true;
                result.Message = "Joined group successfully";
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
        /// Leave group
        /// </summary>
        /// <param name="param">Leave Request Model</param>
        /// <remarks></remarks>
        [Authorize]
        public ResultModel LeaveGroup(LeaveGroupRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                GroupBL bl = new GroupBL();

                bl.Leave(memberId.Value, param);

                result.Status = true;
                result.Message = "Leave group successfully";
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
