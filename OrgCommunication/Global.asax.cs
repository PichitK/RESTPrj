using MultipartDataMediaFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OrgCommunication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.Formatters.Add(new FormMultipartEncodedMediaTypeFormatter());

            //Mail set up
            Helpers.MailSender.Host = Business.Configs.AppConfigs.MailHost;
            Helpers.MailSender.Port = Business.Configs.AppConfigs.MailPort;
            Helpers.MailSender.Username = Business.Configs.AppConfigs.MailUsername;
            Helpers.MailSender.Password = Business.Configs.AppConfigs.MailPassword;
            Helpers.MailSender.EnableSsl = Business.Configs.AppConfigs.MailEnableSsl;
        }
    }
}
