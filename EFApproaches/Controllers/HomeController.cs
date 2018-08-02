using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFApproaches.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page from another PC";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Title = "About Page from another PC";
            return View();
        }
    }
}
