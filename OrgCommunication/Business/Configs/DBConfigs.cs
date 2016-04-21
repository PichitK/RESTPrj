using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgCommunication.Business.Configs
{
    public class DBConfigs
    {
        public static string OrgCommConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["OrgCommEntities"].ConnectionString;
            }
        }
    }
}