using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using OrgCommunication.Business.Configs;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

[assembly: OwinStartup(typeof(OrgCommunication.Startup))]

namespace OrgCommunication
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; set; }

        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
        }

        //ref. http://weblog.west-wind.com/posts/2015/Apr/29/Adding-minimal-OWIN-Identity-Authentication-to-an-Existing-ASPNET-MVC-Application
        public void ConfigureOAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = AppConfigs.OAuthAuthenticationType
            });

            //OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            //{
            //    AllowInsecureHttp = true,
            //    TokenEndpointPath = new PathString("/token"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
            //    Provider = new OrgCommAuthorizationServerProvider()
            //};

            // Token Generation
            //app.UseOAuthAuthorizationServer(OAuthServerOptions);

            Startup.OAuthBearerOptions = new OAuthBearerAuthenticationOptions
            {
                Provider = new OAuthBearerAuthenticationProvider
                {
                    OnRequestToken = async context =>
                    {
                        OrgCommunication.Business.MemberBL bl = new Business.MemberBL();

                        if (!String.IsNullOrWhiteSpace(context.Token))
                        {
                            if (!bl.ValidateToken(context.Token))
                                context.Token = null;
                        }
                    }
                }
            };

            app.UseOAuthBearerAuthentication(Startup.OAuthBearerOptions);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}