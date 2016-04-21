using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Mvc;

namespace OrgCommunication.Filters
{
    public class Authorization : AuthorizeAttribute
    {
        private String _controller;
        private String ControllerName
        {
            get { return _controller; }
        }

        private String _action;
        private String ActionName
        {
            get { return _action; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            _controller = filterContext.Controller.GetType().Name;
            _action = filterContext.ActionDescriptor.ActionName;

            //if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                
            //}
            //else
            //{
            //    //base.OnException(filterContext);
            //}

            base.OnAuthorization(filterContext);
            //if(filterContext.Result==null)
            //{
            //    return;
            //}
            //else if(filterContext.Result.GetType()==typeof(HttpUnauthorizedResult)
            //    && filterContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    filterContext.Result = new ContentResult();
            //    filterContext.HttpContext.Response.StatusCode = 403;
            //}
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            IIdentity id = httpContext.User.Identity;

            //if ((id != null) && (id is IU.Business.Identity.IUIdentity))
            //{
            //    IU.Business.Identity.IUIdentity iuid = (IU.Business.Identity.IUIdentity)HttpContext.Current.User.Identity;

            //    return AuthorizeControl.ValidateAction(iuid, this.ControllerName, this.ActionName);
            //}
            //else
            //{
            //    return false;
            //}

            return base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            IIdentity identity = System.Threading.Thread.CurrentPrincipal.Identity;

            if (identity.IsAuthenticated)
            {
                filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden, "Authorization has been denied for this request.");
            }
            else
            {
                filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized, "Authentication has been denied for this request.");
            }

            filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            //actionContext.Response.Content = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(he));
        }
    }
}