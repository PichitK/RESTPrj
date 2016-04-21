using Microsoft.Owin.Security;
using OrgCommunication.Business.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace OrgCommunication.Helpers.Security
{
    public class IdentityHelper
    {
        public static Models.Member.TokenModel GenerateToken(Models.Member.MemberModel member)
        {
            var claims = new List<Claim>();

            // create required claims
            claims.Add(new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, member.FirstName));

            // custom – my serialized AppUserState object
            //claims.Add(new Claim("userState", member.ToString()));

            var identity = new ClaimsIdentity(claims, AppConfigs.OAuthAuthenticationType);

            DateTime issuredDate = DateTime.UtcNow;
            DateTime expiresDate = issuredDate.Add(TimeSpan.FromHours(AppConfigs.OAuthTimespanHour));
            
            AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            
            ticket.Properties.IssuedUtc = DateTime.SpecifyKind(issuredDate, DateTimeKind.Utc);
            ticket.Properties.ExpiresUtc = DateTime.SpecifyKind(expiresDate, DateTimeKind.Utc);

            string AccessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            return new Models.Member.TokenModel
            {
                MemberId = member.Id,
                Token = AccessToken,
                IssuedUtc = issuredDate,
                ExpiresUtc = expiresDate
            };
        }

        public static int? GetMemberId()
        {
            IPrincipal user = HttpContext.Current.User;

            if ((user == null) || (user.Identity == null)|| !(user.Identity is ClaimsIdentity))
                return null;

            ClaimsIdentity id = (ClaimsIdentity)user.Identity;

            string sId = id.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier)).Value;

            int iId = 0;

            if (Int32.TryParse(sId, out iId))
                return iId;
            else
                return null;
        }
    }
}