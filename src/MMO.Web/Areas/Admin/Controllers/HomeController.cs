﻿using System.Web.Mvc;
using MMO.Web.Areas.Admin.ViewModels;

namespace MMO.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View(new HomeIndex());
        }
    }
}