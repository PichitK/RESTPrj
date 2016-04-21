using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers.Security;
using OrgCommunication.Models;
using OrgCommunication.Models.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace OrgCommunication.APIs
{
    /// <summary>
    /// Member API
    /// </summary>
    public class MemberController : ApiController
    {
        /// <summary>
        /// Revoke authentication token
        /// </summary>
        /// <param name="param">Revoke Auth Request Model</param>
        /// <remarks></remarks>
        public RevokeAuthResultModel RevokeAuth(RevokeAuthRequestModel param)
        {
            RevokeAuthResultModel result = new RevokeAuthResultModel();

            try
            {
                MemberBL bl = new MemberBL();

                var member = bl.GetTokenOwner(new TokenModel { Token = param.AccessToken });
                var token = IdentityHelper.GenerateToken(member);
                
                bl.RegisterToken(token);

                result.AccessToken = token.Token;
                result.Status = true;
                result.Message = "Revoke authentication successfully.";
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
        /// Sign-in and get member information
        /// </summary>
        /// <param name="param">SignIn Request Model</param>
        /// <remarks></remarks>
        public ProfileResultModel SignIn(SignInRequestModel param)
        {
            ProfileResultModel result = new ProfileResultModel();

            if (!this.ModelState.IsValid)
            {
                result.Status = false;
                result.Message = this.ModelState.Values.Single(v => v.Errors.Count > 0).Errors.FirstOrDefault().ErrorMessage;

                return result;
            }

            try
            {
                MemberBL bl = new MemberBL();

                var member = bl.SignIn(param);

                if (member == null)
                {
                    result.Status = false;
                    result.Message = "Invalid user name";
                }
                else
                {
                    var token = IdentityHelper.GenerateToken(member);

                    bl.RegisterToken(token);
                    
                    result.Status = true;
                    result.Message = "Sign-in successfully";
                    result.Member = member;
                    result.Member.AccessToken = token.Token;
                }
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
        /// Sign-in and get member information with Facebook
        /// </summary>
        /// <param name="param">SignIn With Facebook Request Model</param>
        /// <remarks></remarks>
        public ProfileResultModel SignInWithFacebook(SignInWithFacebookRequestModel param)
        {
            ProfileResultModel result = new ProfileResultModel();

            if (!this.ModelState.IsValid)
            {
                result.Status = false;
                result.Message = this.ModelState.Values.Single(v => v.Errors.Count > 0).Errors.FirstOrDefault().ErrorMessage;

                return result;
            }

            try
            {
                MemberBL bl = new MemberBL();

                var member = bl.SignInWithFacebookId(param);

                if (member == null)
                {
                    result.Status = false;
                    result.Message = "Invalid facebook Id";
                }
                else
                {
                    var token = IdentityHelper.GenerateToken(member);

                    bl.RegisterToken(token);

                    result.Status = true;
                    result.Message = "Sign-in successfully";
                    result.Member = member;
                    result.Member.AccessToken = token.Token;
                }
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
        /// Sign out
        /// </summary>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [Authorize]
        public ResultModel SignOut()
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                MemberBL bl = new MemberBL();

                bl.SignOut(memberId.Value);

                result.Status = true;
                result.Message = "Sign-out successfully";
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
        /// Activate member
        /// </summary>
        /// <param name="param">Activate Request Model</param>
        /// <remarks></remarks>
        public ActivateResultModel Activate(ActivateRequestModel param)
        {
            ActivateResultModel result = new ActivateResultModel();

            try
            {
                MemberBL bl = new MemberBL();

                var member = bl.Activate(param);
                var token = IdentityHelper.GenerateToken(member);

                bl.RegisterToken(token);

                result.AccessToken = token.Token;
                result.Status = true;
                result.Message = "Activated!";
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
        /// Register member information
        /// </summary>
        /// <param name="param">Register Request Model</param>
        /// <remarks></remarks>
        [SwaggerConfig.SwashConsumeMultipart(typeof(RegisterRequestModel))]
        public ProfileResultModel Register(RegisterRequestModel param)
        {
            ProfileResultModel result = new ProfileResultModel();

            try
            {
                MemberBL bl = new MemberBL();

                string activationKey = null;
                var member = bl.Register(param, out activationKey);

                result.Status = true;
                //result.Message = "Registered";
                /*** TEMPORARY ***/
                result.Message = activationKey;

                result.Member = member;

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
        /// Get profile
        /// </summary>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [HttpPost]
        [Authorize]
        public ProfileResultModel GetProfile()
        {
            ProfileResultModel result = new ProfileResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                MemberBL bl = new MemberBL();

                var memberList = bl.GetMemberByCriteria(memberId.Value.ToString(), MemberBL.ExistCriteriaType.Id);

                if (memberList.Count == 0)
                    throw new OrgException("Invalid profile");

                result.Status = true;
                result.Member = memberList[0].Value;
                result.Message = "Get profile successfully";
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
        /// Update member information
        /// </summary>
        /// <param name="param">Update Profile Request Model</param>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [SwaggerConfig.SwashConsumeMultipart(typeof(UpdateProfileRequestModel))]
        [Authorize]
        public ProfileResultModel UpdateProfile(UpdateProfileRequestModel param)
        {
            ProfileResultModel result = new ProfileResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                MemberBL bl = new MemberBL();

                var member = bl.UpdateProfile(memberId.Value, new UpdateMemberModel
                {
                    Email = param.Email,
                    FirstName = param.FirstName,
                    LastName = param.LastName,
                    NickName = param.NickName,
                    Gender = param.Gender,
                    Photo = param.Photo
                });

                result.Status = true;
                result.Message = "Updated profile successfully.";
                result.Member = member;

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
        /// Update member's display name
        /// </summary>
        /// <param name="param">Update Display Name Request Model</param>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [SwaggerConfig.SwashConsumeMultipart(typeof(UpdateDisplayNameRequestModel))]
        [Authorize]
        public ProfileResultModel UpdateDisplayName(UpdateDisplayNameRequestModel param)
        {
            ProfileResultModel result = new ProfileResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                MemberBL bl = new MemberBL();

                var member = bl.UpdateProfile(memberId.Value, new UpdateMemberModel
                {
                    DisplayName = (param == null)? null : param.DisplayName
                });

                result.Status = true;
                result.Message = "Updated profile successfully.";
                result.Member = member;

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
        /// Update member's phone no.
        /// </summary>
        /// <param name="param">Update Phone no Request Model</param>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [SwaggerConfig.SwashConsumeMultipart(typeof(UpdatePhoneNoRequestModel))]
        [Authorize]
        public ProfileResultModel UpdatePhoneNo(UpdatePhoneNoRequestModel param)
        {
            ProfileResultModel result = new ProfileResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                MemberBL bl = new MemberBL();

                var member = bl.UpdateProfile(memberId.Value, new UpdateMemberModel
                {
                    Phone = (param == null) ? null : param.Phone
                });

                result.Status = true;
                result.Message = "Updated phone no. successfully.";
                result.Member = member;

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
        /// Change password
        /// </summary>
        /// <param name="param">Change password Request Model</param>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [Authorize]
        public ResultModel ChangePassword(ChangePasswordRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                MemberBL bl = new MemberBL();

                bl.ChangePassword(memberId.Value, param);

                result.Status = true;
                result.Message = "Your password has been changed";
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
        /// Reset password
        /// </summary>
        /// <param name="param">Reset password Request Model</param>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        public ResultModel ResetPassword(ResetPasswordRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                MemberBL bl = new MemberBL();

                bl.ResetPassword(param);

                result.Status = true;
                result.Message = "Your password has been changed";
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
        /// Delete account
        /// </summary>
        /// <param name="param">Delete Account Request Model</param>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [HttpPost]
        [Authorize]
        public ResultModel DeleteAccount(DeleteAccountRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid MemberId");

                MemberBL bl = new MemberBL();

                bl.RemoveMember(memberId.Value, param);

                result.Status = true;
                result.Message = "Your account has been deleted";
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
        /// Check if email is already exists
        /// </summary>
        /// <param name="email">email</param>
        /// <remarks></remarks>
        [HttpPost]
        public ResultModel IsEmailAlreadyExists(string email)
        {
            ResultModel result = new ResultModel();

            try
            {
                MemberBL bl = new MemberBL();

                if (bl.IsEmailAlreadyExists(email))
                {
                    result.Status = true;
                    result.Message = "email '" + email + "' already exists";
                }
                else
                {
                    result.Status = false;
                    result.Message = "email '" + email + "' not exists";
                }
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
        /// Check if member exist by email
        /// </summary>
        /// <param name="email">Email; Use comma "," for many email, Ex: first@copa.co.th,second@copa.co.th</param>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [HttpPost]
        [Authorize]
        public ExistResultModel CheckIfMemberExistsByEmail(string email)
        {
            ExistResultModel result = new ExistResultModel();

            try
            {
                MemberBL bl = new MemberBL();

                var memberByEmail = bl.GetMemberByCriteria(email, MemberBL.ExistCriteriaType.Email);

                result.Items = memberByEmail.Select(r => new ExistModel
                {
                    Criteria = r.Key,
                    IsExists = (r.Value != null),
                    Id = (r.Value == null)? null : (int?)r.Value.Id
                }).ToList();

                result.Status = true;
                result.Message = "Checked";
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
        /// Check if phone no is already exists
        /// </summary>
        /// <param name="phoneNo">Phone number</param>
        /// <remarks></remarks>
        [HttpPost]
        public ResultModel IsPhoneNoAlreadyExists(string phoneNo)
        {
            ResultModel result = new ResultModel();

            try
            {
                MemberBL bl = new MemberBL();

                if (bl.IsPhoneNoAlreadyExists(phoneNo))
                {
                    result.Status = true;
                    result.Message = "Phone No. '" + phoneNo + "' already exists";
                }
                else
                {
                    result.Status = false;
                    result.Message = "Phone No. '" + phoneNo + "' not exists";
                }
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
        /// Check if member exist by email
        /// </summary>
        /// <param name="phoneNo">Phone No.; Use comma "," for many number, Ex: 0898918096,087984567,087984777</param>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [HttpPost]
        [Authorize]
        public ExistResultModel CheckIfMemberExistsByPhone(string phoneNo)
        {
            ExistResultModel result = new ExistResultModel();

            try
            {
                MemberBL bl = new MemberBL();

                var memberByEmail = bl.GetMemberByCriteria(phoneNo, MemberBL.ExistCriteriaType.PhoneNo);

                result.Items = memberByEmail.Select(r => new ExistModel
                {
                    Criteria = r.Key,
                    IsExists = (r.Value != null),
                    Id = (r.Value == null) ? null : (int?)r.Value.Id
                }).ToList();

                result.Status = true;
                result.Message = "Checked";
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
        /// Add device
        /// </summary>
        /// <param name="param">Device Token Request Model</param>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        [Authorize]
        public ResultModel RegisterDeviceToken(DeviceTokenRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                int? memberId = IdentityHelper.GetMemberId();
                if (!memberId.HasValue)
                    throw new OrgException("Invalid member MemberId");

                MemberBL bl = new MemberBL();

                bl.RegisterDevice(memberId.Value, param);

                result.Status = true;
                result.Message = "Your device token has been registered";
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
        /// Remove device
        /// </summary>
        /// <param name="param">Device Token Request Model</param>
        /// <remarks>Use access token of member model for request authorized API</remarks>
        public ResultModel RevokeDeviceToken(DeviceTokenRequestModel param)
        {
            ResultModel result = new ResultModel();

            try
            {
                MemberBL bl = new MemberBL();

                bl.RevokeDevice(param);

                result.Status = true;
                result.Message = "Your token has been revoked";
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