using MMO.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MMO.Web.Controllers
{
    [Authorize(Roles = "registered")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new HomeIndex());
        }
    }
}