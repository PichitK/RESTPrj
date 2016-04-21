using Microsoft.Owin.Security;
using OrgCommunication.Business;
using OrgCommunication.Business.Configs;
using OrgCommunication.Business.Exception;
using OrgCommunication.Helpers.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;


namespace OrgCommunication.Controllers
{
    public class MemberController : Business.Abstract.BaseController
    {
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Activate(Models.Member.ActivateRequestModel param)
        {
            Models.Member.ActivateResultModel result = new Models.Member.ActivateResultModel();

            try
            {
                MemberBL bl = new MemberBL();

                bl.Activate(param);

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

            return View(result);
        }
        
        public FileResult Photo(Models.Member.PhotoRequestModel param)
        {
            try
            {
                MemberBL bl = new MemberBL();

                byte[] photo = bl.GetMemberPhoto(param);

                return this.CreateImageFileResult(photo);
            }
            catch (OrgException oex)
            {
                throw new HttpException((int)System.Net.HttpStatusCode.NotFound, oex.Message);
            }
            catch (Exception ex)
            {
                if (AppConfigs.DebugInternalMessage)
                    throw new HttpException((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
                else
                    throw new HttpException((int)System.Net.HttpStatusCode.NotFound, AppConfigs.InternalErrorMessage);
            }
        }

        #region Identity
        private void IdentitySignin(Models.Member.MemberModel member, bool isPersistent = false)
        {
            var claims = new List<Claim>();

            // create required claims
            claims.Add(new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, member.FirstName));

            // custom – my serialized AppUserState object
            //claims.Add(new Claim("userState", member.ToString()));

            var identity = new ClaimsIdentity(claims, AppConfigs.OAuthAuthenticationType);

            AuthenticationManager.SignIn(new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            }, identity);
        }

        private void IdentitySignout()
        {
            AuthenticationManager.SignOut(new string[] { AppConfigs.OAuthAuthenticationType });
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
        }
        #endregion
    }
}