using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMO.Web.Areas.Admin.ViewModels;

namespace MMO.Web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        public ActionResult Index()
        {
            return View(new UserIndex());
        }

        public ActionResult Create() {
            return View(new UserCreate());
        }

        [HttpPost]
        public ActionResult Create(UserCreate from) {
            return View(from);
        }
    }
}