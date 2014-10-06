using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMO.Data;
using MMO.Data.Entities;
using MMO.Web.Areas.Admin.ViewModels;

namespace MMO.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class DeployTokensController : Controller
    {
        private readonly MMODatabseContext _database = new MMODatabseContext();
        public ActionResult Index() {
            return View(new DeployTokensIndex {
                Tokens = _database.DeployTokens.OrderByDescending(t => t.CreatedAt).ToList()
            });
        }

        public ActionResult Create() {
            return View(new DeployTokensCreate());
        }

        [HttpPost]
        public ActionResult Create(DeployTokensCreate form) {
            if (!ModelState.IsValid) {
                return View(form);
            }

            var token = new DeployToken(form.IpAddress);
            _database.DeployTokens.Add(token);
            _database.SaveChanges();
            return View(form);
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            var token = _database.DeployTokens.Find(id);
            if (token == null) {
                return HttpNotFound();
            }

            _database.DeployTokens.Remove(token);
            _database.SaveChanges();

            return RedirectToAction("index");
        }
    }
}