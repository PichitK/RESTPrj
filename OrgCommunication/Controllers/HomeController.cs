using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrgCommunication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //OrgComm.Data.ChatEntities c = new OrgComm.Data.ChatEntities();

            //c.test();

            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}