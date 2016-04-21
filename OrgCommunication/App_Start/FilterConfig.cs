using System.Web;
using System.Web.Mvc;

namespace OrgCommunication
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new OrgCommunication.Filters.GlobalExceptionHandleAttribute());
        }
    }
}
