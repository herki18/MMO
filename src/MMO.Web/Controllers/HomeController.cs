using MMO.Data;
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
        private readonly MMODatabseContext _database = new MMODatabseContext();
        public ActionResult Index() {
            return View(new HomeIndex() {
                LatestLauncher = _database.Launchers.OrderByDescending(t => t.Version.Version).ThenByDescending(f => f.Version.Timestamp).FirstOrDefault()
            });
        }
    }
}