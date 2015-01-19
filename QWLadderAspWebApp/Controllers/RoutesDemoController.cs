using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QWLadderAspWebApp.Controllers
{
    public class RoutesDemoController : Controller
    {
        public ActionResult One()
        {
            return View();
        }

        public ActionResult Two(int fred = 1)
        {
            ViewBag.Freddy = fred;

            return View();
        }

        public ActionResult Three()
        {
            return View();
        }

        public ActionResult Four()
        {
            return View();
        }
    }
}