using OrgComm.Data;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers;
using OrgCommunication.Helpers.Drawing;
using OrgCommunication.Helpers.Security;
using OrgCommunication.Models.Member;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace OrgCommunication.Business
{
    public class MemberBL
    {
        public enum ExistCriteriaType
        {
            Id,
            Email,
            PhoneNo
        }

        private static string _photoUrlFormatString = null;
        public static string PhotoUrlFormatString
        {
            get
            {
                string url = null;

                if (System.Web.HttpContext.Current == null)
                {
                    return "/Member/Photo?Id={0}";
                }
                else
                {
                    if (_photoUrlFormatString == null)
                        _photoUrlFormatString = string.Format("{0}{1}{2}",
                        HttpContext.Current.Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped),
                        (HttpContext.Current.Request.ApplicationPath.Equals("/")) ? "/" : HttpContext.Current.Request.ApplicationPath
                        , "Member/Photo?Id={0}");

                    return _photoUrlFormatString;
                }
            }
        }

        public MemberBL()
        {

        }

        public bool IsEmailAlreadyExists(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new OrgException("Invalid e-mail");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                return dbc.Members.Any(r => ((!r.DelFlag) && r.Email.Equals(email)));
            }
        }

        public bool IsPhoneNoAlreadyExists(string phoneNo)
        {
            if (String.IsNullOrWhiteSpace(phoneNo))
                throw new OrgException("Invalid phone no.");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                return dbc.Members.Any(r => ((!r.DelFlag) && r.Phone.Equals(phoneNo)));
            }
        }

        public List<KeyValuePair<string, MemberModel>> GetMemberByCriteria(string criteria, ExistCriteriaType type)
        {
            if (String.IsNullOrWhiteSpace(criteria))
                throw new OrgException("Invalid criteria");

            List<KeyValuePair<string, MemberModel>> memberByCriteria = new List<KeyValuePair<string, MemberModel>>();

            string[] arrCriteria = criteria.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                MemberModel m = null;

                foreach (string s in arrCriteria)
                {
                    if (String.IsNullOrWhiteSpace(s))
                        continue;

                    var qry = dbc.Members.Where(r => !r.DelFlag);

                    string key = s.Trim();
                    m = null;

                    if (type == ExistCriteriaType.Id)
                        qry = qry.Where(r => r.Id.ToString().Equals(key));
                    else if (type == ExistCriteriaType.Email)
                        qry = qry.Where(r => r.Email.Equals(key));
                    else
                        qry = qry.Where(r => r.Phone.Equals(key));

                    m = qry.Select(r => new MemberModel
                    {
                        Id = r.Id,
                        FacebookId = r.FacebookId,
                        Email = r.Email,
                        FirstName = r.FirstName,
                        LastName = r.LastName,
                        NickName = r.Nickname,
                        DisplayName = r.DisplayName,
                        Gender = r.Gender,
                        Company = r.Company.Name,
                        Department = r.Department.Name,
                        Position = r.Position.Name,
                        EmployeeId = r.EmployeeId,
                        Phone = r.Phone,
                        Photo = (r.Photo == null) ? null : MemberBL.PhotoUrlFormatString.Replace("{0}", r.Id.ToString())
                    }).SingleOrDefault();

                    memberByCriteria.Add(new KeyValuePair<string, MemberModel>(key, m));
                }
            }

            return memberByCriteria;
        }

        public MemberModel Register(RegisterRequestModel model, out string activationKey)
        {
            OrgComm.Data.Models.Member member = new OrgComm.Data.Models.Member
            {
                FacebookId = model.FacebookId,
                MemberStatus = (int)OrgComm.Data.Models.Member.StatusType.New,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Nickname = model.NickName,
                Gender = model.Gender,
                EmployeeId = model.EmployeeId,
                RegisteredDate = DateTime.Now,
            };

            if (!String.IsNullOrWhiteSpace(model.Email))
            {
                Validator validator = new Validator();
                string email = model.Email.Trim();

                if (!validator.IsValidEmail(email))
                    throw new OrgException("Invalid e-mail");

                member.Email = email;
            }

            if (!model.CompanyId.HasValue)
                throw new OrgException("Invalid company");

            if (!model.DepartmentId.HasValue)
                throw new OrgException("Invalid department");

            if (!model.PositionId.HasValue)
                throw new OrgException("Invalid position");

            if (!String.IsNullOrWhiteSpace(model.Phone))
            {
                string[] groups = model.Phone.Split(new[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                member.Phone = String.Join("", groups);
            }

            if (model.Photo != null)
            {
                byte[] photo = model.Photo.Buffer;

                using (System.IO.MemoryStream msReader = new System.IO.MemoryStream(photo))
                {
                    using (System.Drawing.Image img = System.Drawing.Image.FromStream(msReader))
                    {
                        string imageType = ImageHelper.GetImageFormat(img);

                        if (imageType == null)
                            throw new OrgException("Not support image type");

                        int? width, height;
                        Image imgResize = null;

                        //Size Max constraint
                        width = AppConfigs.MemberPhotoWidthMax;
                        height = AppConfigs.MemberPhotoHeightMax;

                        imgResize = ImageHelper.ReSize(img, width, height, ImageHelper.ResizeMode.KeepAspectRatio);
                        member.Photo = ImageHelper.ImageToByteArray(imgResize, img.RawFormat);
                    }
                }
            }

            member.Salt = SecurityHelper.GenerateBase64SaltString();
            member.PasswordHash = this.GenerateHash(member.Salt, model.Password);

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                if (!String.IsNullOrWhiteSpace(member.FacebookId) && dbc.Members.Any(r => ((!r.DelFlag) && r.FacebookId.Equals(member.FacebookId))))
                    throw new OrgException("Duplicate FacebookId");

                if (!string.IsNullOrWhiteSpace(model.Email))
                {
                    if (dbc.Members.Any(r => ((!r.DelFlag) && r.Email.Equals(model.Email))))
                        throw new OrgException("Duplicate e-mail");
                }

                if (!string.IsNullOrWhiteSpace(model.Phone))
                {
                    if (dbc.Members.Any(r => ((!r.DelFlag) && r.Phone.Equals(model.Phone))))
                        throw new OrgException("Duplicate phone no.");
                }

                var company = dbc.Company.FirstOrDefault(r => r.Id.Equals(model.CompanyId.Value));
                if (company == null)
                    throw new OrgException("Invalid company");

                member.Company = company;

                var department = dbc.Department.FirstOrDefault(r => r.Id.Equals(model.DepartmentId.Value));
                if (department == null)
                    throw new OrgException("Invalid department");

                member.Department = department;

                var position = dbc.Position.FirstOrDefault(r => r.Id.Equals(model.PositionId.Value));
                if (position == null)
                    throw new OrgException("Invalid position");

                member.Position = position;

                member.ActivationKey = Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(System.Web.Security.Membership.GeneratePassword(6, 0)));

                dbc.Members.Add(member);

                dbc.SaveChanges();

                activationKey = member.ActivationKey;

                if (AppConfigs.MailSendMail)
                {
                    System.Collections.Specialized.ListDictionary listReplacement = new System.Collections.Specialized.ListDictionary();

                    listReplacement.Add("{id}", member.Id.ToString());
                    listReplacement.Add("{activationkey}", member.ActivationKey);

                    MailSender.Send(AppConfigs.MailFrom, member.Email, "Activate account", listReplacement, AppConfigs.MailTemplateActivate);
                }
            }

            return new MemberModel
            {
                Id = member.Id,
                FacebookId = member.FacebookId,
                Email = member.Email,
                FirstName = member.FirstName,
                LastName = member.LastName,
                NickName = member.Nickname,
                DisplayName = member.DisplayName,
                Gender = member.Gender,
                Company = member.Company.Name,
                Department = member.Department.Name,
                Position = member.Position.Name,
                EmployeeId = member.EmployeeId,
                Phone = member.Phone,
                Photo = (member.Photo == null) ? null : MemberBL.PhotoUrlFormatString.Replace("{0}", member.Id.ToString())
            };
        }

        public MemberModel Activate(ActivateRequestModel model)
        {
            OrgComm.Data.Models.Member member = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                member = dbc.Members.SingleOrDefault(r => (!r.DelFlag) && r.Id.Equals(model.MemberId));

                if (member == null)
                    throw new OrgException(1, "Invalid profile");

                if (member.MemberStatus != (int)OrgComm.Data.Models.Member.StatusType.New)
                    throw new OrgException(1, "Invalid activation key");

                if (!member.ActivationKey.Equals(model.ActivationKey))
                    throw new OrgException(2, "Invalid activation key");

                member.MemberStatus = (int)OrgComm.Data.Models.Member.StatusType.Activated;

                if (!model.DeviceOSId.HasValue || String.IsNullOrWhiteSpace(model.DeviceToken))
                    throw new OrgException("Invalid device token");

                this.RegisterDevice(member.Id, new DeviceTokenRequestModel
                {
                    OSId = model.DeviceOSId.Value,
                    Token = model.DeviceToken
                });

                dbc.SaveChanges();
            }

            return new MemberModel
            {
                Id = member.Id,
                FacebookId = member.FacebookId,
                Email = member.Email,
                FirstName = member.FirstName,
                LastName = member.LastName,
                NickName = member.Nickname,
                DisplayName = member.DisplayName,
                Gender = member.Gender,
                Company = member.Company.Name,
                Department = member.Department.Name,
                Position = member.Position.Name,
                EmployeeId = member.EmployeeId,
                Phone = member.Phone,
                Photo = (member.Photo == null) ? null : MemberBL.PhotoUrlFormatString.Replace("{0}", member.Id.ToString())
            };
        }

        public MemberModel UpdateProfile(int memberId, UpdateMemberModel model)
        {
            OrgComm.Data.Models.Member member = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                member = dbc.Members.SingleOrDefault(r => (!r.DelFlag) && r.Id.Equals(memberId));

                if (member == null)
                    throw new OrgException(1, "Invalid profile");

                if (model.Photo == null)
                {
                    member.Photo = null;
                }
                else
                {
                    byte[] photo = model.Photo.Buffer;

                    using (System.IO.MemoryStream msReader = new System.IO.MemoryStream(photo))
                    {
                        using (System.Drawing.Image img = System.Drawing.Image.FromStream(msReader))
                        {
                            string imageType = ImageHelper.GetImageFormat(img);

                            if (imageType == null)
                                throw new OrgException("Not support image type");

                            int? width, height;
                            Image imgResize = null;

                            //Size Max constraint
                            width = AppConfigs.MemberPhotoWidthMax;
                            height = AppConfigs.MemberPhotoHeightMax;

                            imgResize = ImageHelper.ReSize(img, width, height, ImageHelper.ResizeMode.KeepAspectRatio);
                            member.Photo = ImageHelper.ImageToByteArray(imgResize, img.RawFormat);
                        }
                    }
                }

                if (!String.IsNullOrWhiteSpace(model.Email))
                {
                    Validator validator = new Validator();
                    string email = model.Email.Trim();

                    if (!validator.IsValidEmail(email))
                        throw new OrgException("Invalid e-mail");

                    if (dbc.Members.Any(r => (r.Id != memberId) && (r.Email.Equals(email))))
                        throw new OrgException("Duplicate e-mail");

                    member.Email = email;
                }

                if (!String.IsNullOrWhiteSpace(model.FirstName))
                    member.FirstName = model.FirstName;

                if (!String.IsNullOrWhiteSpace(model.LastName))
                    member.LastName = model.LastName;

                if (!String.IsNullOrWhiteSpace(model.DisplayName))
                    member.DisplayName = model.DisplayName;

                if (!String.IsNullOrWhiteSpace(model.Gender))
                    member.Gender = model.Gender;

                if (!String.IsNullOrWhiteSpace(model.Phone))
                {
                    if (dbc.Members.Any(r => (r.Id != memberId) && (r.Phone.Equals(model.Phone))))
                        throw new OrgException("Duplicate phone no.");

                    member.Phone = model.Phone;
                }

                member.UpdatedDate = DateTime.Now;

                dbc.SaveChanges();
            }

            return new MemberModel
            {
                Id = member.Id,
                FacebookId = member.FacebookId,
                Email = member.Email,
                FirstName = member.FirstName,
                LastName = member.LastName,
                NickName = member.Nickname,
                DisplayName = member.DisplayName,
                Gender = member.Gender,
                Company = member.Company.Name,
                Department = member.Department.Name,
                Position = member.Position.Name,
                EmployeeId = member.EmployeeId,
                Phone = member.Phone,
                Photo = (member.Photo == null) ? null : MemberBL.PhotoUrlFormatString.Replace("{0}", member.Id.ToString())
            };
        }

        public void ChangePassword(int memberId, ChangePasswordRequestModel model)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                OrgComm.Data.Models.Member member = dbc.Members.SingleOrDefault(r => (!r.DelFlag) && r.Id.Equals(memberId));

                if (member == null)
                    throw new OrgException("Invalid profile");

                if (!member.PasswordHash.Equals(GenerateHash(member.Salt, model.CurrentPassword)))
                    throw new OrgException("Incorrect password");

                string password = model.NewPassword;

                member.Salt = SecurityHelper.GenerateBase64SaltString();
                member.PasswordHash = this.GenerateHash(member.Salt, password);

                member.UpdatedDate = DateTime.Now;

                dbc.SaveChanges();
            }
        }

        public void ResetPassword(ResetPasswordRequestModel model)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                OrgComm.Data.Models.Member member = dbc.Members.SingleOrDefault(r => (!r.DelFlag) && r.Email.Equals(model.Email));

                if (member == null)
                    throw new OrgException(1, "Invalid profile");

                string password = System.Web.Security.Membership.GeneratePassword(8, 0);

                member.Salt = SecurityHelper.GenerateBase64SaltString();
                member.PasswordHash = this.GenerateHash(member.Salt, password);

                member.UpdatedDate = DateTime.Now;

                dbc.SaveChanges();

                if (AppConfigs.MailSendMail)
                {
                    System.Collections.Specialized.ListDictionary listReplacement = new System.Collections.Specialized.ListDictionary();

                    listReplacement.Add("{password}", password);

                    MailSender.Send(AppConfigs.MailFrom, member.Email, "Reset password", listReplacement, AppConfigs.MailTemplateResetPassword);
                }
            }
        }

        public byte[] GetMemberPhoto(PhotoRequestModel model)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var qry = dbc.Members.AsQueryable();
                OrgComm.Data.Models.Member member = null;

                if (model.Id.HasValue)
                {
                    member = qry.Where(r => (!r.DelFlag) && (r.Id == model.Id.Value)).FirstOrDefault();
                }
                else
                {
                    throw new OrgException("Invalid id");
                }

                if (member == null)
                    throw new OrgException("Member not found");
                else
                {
                    if (member.Photo == null)
                        throw new OrgException("Photo not found");

                    return member.Photo;
                }

            }
        }

        public MemberModel SignIn(SignInRequestModel model)
        {
            OrgComm.Data.Models.Member member = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                member = dbc.Members.FirstOrDefault(r => (!r.DelFlag) && (r.Email.Equals(model.Email)));

                if ((member == null) || !member.PasswordHash.Equals(GenerateHash(member.Salt, model.Password)))
                    return null;

                if (!model.DeviceOSId.HasValue || String.IsNullOrWhiteSpace(model.DeviceToken))
                    throw new OrgException("Invalid device token");

                this.RegisterDevice(member.Id, new DeviceTokenRequestModel
                {
                    OSId = model.DeviceOSId.Value,
                    Token = model.DeviceToken
                });
            }

            return new MemberModel
            {
                Id = member.Id,
                FacebookId = member.FacebookId,
                Email = member.Email,
                FirstName = member.FirstName,
                LastName = member.LastName,
                NickName = member.Nickname,
                DisplayName = member.DisplayName,
                Gender = member.Gender,
                Company = member.Company.Name,
                Department = member.Department.Name,
                Position = member.Position.Name,
                EmployeeId = member.EmployeeId,
                Phone = member.Phone,
                Photo = (member.Photo == null) ? null : MemberBL.PhotoUrlFormatString.Replace("{0}", member.Id.ToString())
            };
        }

        public MemberModel SignInWithFacebookId(SignInWithFacebookRequestModel model)
        {
            OrgComm.Data.Models.Member member = null;

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                member = dbc.Members.FirstOrDefault(r => (!r.DelFlag) && (r.FacebookId.Equals(model.FacebookId)));

                if (member == null)
                    return null;

                if (!model.DeviceOSId.HasValue || String.IsNullOrWhiteSpace(model.DeviceToken))
                    throw new OrgException("Invalid device token");

                this.RegisterDevice(member.Id, new DeviceTokenRequestModel
                {
                    OSId = model.DeviceOSId.Value,
                    Token = model.DeviceToken
                });
            }

            return new MemberModel
            {
                Id = member.Id,
                FacebookId = member.FacebookId,
                Email = member.Email,
                FirstName = member.FirstName,
                LastName = member.LastName,
                NickName = member.Nickname,
                DisplayName = member.DisplayName,
                Gender = member.Gender,
                Company = member.Company.Name,
                Department = member.Department.Name,
                Position = member.Position.Name,
                EmployeeId = member.EmployeeId,
                Phone = member.Phone,
                Photo = (member.Photo == null) ? null : MemberBL.PhotoUrlFormatString.Replace("{0}", member.Id.ToString())
            };
        }

        public void SignOut(int memberId)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var tk = dbc.Tokens.SingleOrDefault(r => r.MemberId.Equals(memberId));

                if (tk != null)
                {
                    dbc.Tokens.Remove(tk);
                    dbc.SaveChanges();
                }
            }
        }

        public void RemoveMember(int memberId, DeleteAccountRequestModel model)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                OrgComm.Data.Models.Member member = dbc.Members.SingleOrDefault(r => (!r.DelFlag) && r.Id.Equals(memberId));

                if (member == null)
                    throw new OrgException("Invalid profile");

                if (!member.PasswordHash.Equals(GenerateHash(member.Salt, model.Password)))
                    throw new OrgException("Incorrect password");

                member.DelFlag = true;

                var tokens = dbc.Tokens.Where(r => r.MemberId.Equals(memberId));
                if (tokens.Count() > 0)
                    dbc.Tokens.RemoveRange(tokens);

                var devices = dbc.Devices.Where(r => r.MemberId.Equals(memberId));
                if (devices.Count() > 0)
                    dbc.Devices.RemoveRange(devices);

                new FriendBL().RemoveFriendByMemberId(member.Id);
                new GroupBL().LeaveAllGroupsByMemberId(member.Id);
                new UploadBL().RemoveFilesByMemberId(member.Id);
                new NoteBL().RemoveNoteByMemberId(member.Id);

                dbc.SaveChanges();
            }
        }

        public MemberModel GetTokenOwner(TokenModel model)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var token = dbc.Tokens.SingleOrDefault(r => r.Token.Equals(model.Token));

                if (token == null)
                {
                    throw new OrgException("Invalid access token");
                }
                else
                {
                    var member = dbc.Members.SingleOrDefault(r => r.Id.Equals(token.MemberId) && (!r.DelFlag));

                    if (member == null)
                        throw new OrgException("Invalid member");
                    else
                        return new MemberModel
                        {
                            Id = member.Id,
                            FacebookId = member.FacebookId,
                            Email = member.Email,
                            FirstName = member.FirstName,
                            LastName = member.LastName,
                            NickName = member.Nickname,
                            DisplayName = member.DisplayName,
                            Gender = member.Gender,
                            Company = member.Company.Name,
                            Department = member.Department.Name,
                            Position = member.Position.Name,
                            EmployeeId = member.EmployeeId,
                            Phone = member.Phone,
                            Photo = (member.Photo == null) ? null : MemberBL.PhotoUrlFormatString.Replace("{0}", member.Id.ToString())
                        };
                }
            }
        }

        public void RegisterToken(TokenModel model)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var token = dbc.Tokens.SingleOrDefault(r => r.MemberId.Equals(model.MemberId));

                if (token == null)
                {
                    dbc.Tokens.Add(new OrgComm.Data.Models.MemberToken
                    {
                        MemberId = model.MemberId,
                        Token = model.Token,
                        IssuedUtc = model.IssuedUtc,
                        ExpiresUtc = model.ExpiresUtc
                    });
                }
                else
                {
                    token.Token = model.Token;
                    token.IssuedUtc = model.IssuedUtc;
                    token.ExpiresUtc = model.ExpiresUtc;
                }

                dbc.SaveChanges();
            }
        }

        public void RevokeToken(TokenModel model)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var token = dbc.Tokens.SingleOrDefault(r => r.MemberId.Equals(model.MemberId));

                if (token != null)
                {
                    dbc.Tokens.Remove(token);
                }

                dbc.SaveChanges();
            }
        }

        public bool ValidateToken(string token)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var tk = dbc.Tokens.SingleOrDefault(r => r.Token.Equals(token));

                if (tk == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public void RegisterDevice(int memberId, DeviceTokenRequestModel model)
        {
            if (!model.OSId.HasValue)
                throw new OrgException("Invalid device OS Id");

            if (String.IsNullOrWhiteSpace(model.Token))
                throw new OrgException("Invalid device token");

            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var device = dbc.Devices.SingleOrDefault(r => r.MemberId.Equals(memberId));

                if (device != null)
                {
                    dbc.Devices.Remove(device);
                    dbc.SaveChanges();
                }

                device = dbc.Devices.SingleOrDefault(r => r.OSId.Equals(model.OSId.Value) && r.Token.Equals(model.Token));

                if (device == null)
                {
                    dbc.Devices.Add(new OrgComm.Data.Models.MemberDevice
                    {
                        MemberId = memberId,
                        OSId = model.OSId.Value,
                        Token = model.Token,
                        CreatedDate = DateTime.Now
                    });
                }
                else
                {
                    throw new OrgException("Device token is already registered");
                }

                dbc.SaveChanges();
            }
        }

        public void RevokeDevice(DeviceTokenRequestModel model)
        {
            using (OrgCommEntities dbc = new OrgCommEntities(DBConfigs.OrgCommConnectionString))
            {
                var device = dbc.Devices.SingleOrDefault(r => r.OSId.Equals(model.OSId) && r.Token.Equals(model.Token));

                if (device != null)
                    dbc.Devices.Remove(device);

                dbc.SaveChanges();
            }
        }

        #region Private Method
        private string GenerateHash(string salt, string plaintext)
        {
            return SecurityHelper.ComputeBase64Hash(salt + plaintext);
        }

        private string RemoveNonNumeric(string text)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(text, "");
        }
        #endregion
    }
}