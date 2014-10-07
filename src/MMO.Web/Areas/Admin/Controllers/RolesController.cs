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
    public class RolesController : Controller
    {
        private readonly MMODatabseContext _database = new MMODatabseContext();

        public ActionResult Index() {
            return View(new RolesIndex() {
                Roles = _database.Roles.ToList()
            });
        }

        public ActionResult Create() {
            return View(new RolesCreate());
        }

        [HttpPost]
        public ActionResult Create(RolesCreate form) {

            if (!ModelState.IsValid) {
                return View(form);
            }

            var role = new Role {
                Name = form.Name,
                IsUserDefined = true
            };

            _database.Roles.Add(role);
            _database.SaveChanges();

            return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            var role = _database.Roles.Find(id);
            if (role == null || role.CanEditAndDelete) {
                return HttpNotFound();
            }

            _database.Roles.Remove(role);
            _database.SaveChanges();

            return RedirectToAction("index");
        }

        public ActionResult Edit(int id) {
            var role = _database.Roles.Find(id);
            if (role == null || !role.CanEditAndDelete) {
                return HttpNotFound();
            }

            return View(new RolesEdit() {
                Name = role.Name
            });
        }

        [HttpPost]
        public ActionResult Edit(int id, RolesEdit form) {

            var role = _database.Roles.Find(id);
            if (role == null || role.CanEditAndDelete)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(form);
            }

            role.Name = form.Name;
            _database.SaveChanges();

            return RedirectToAction("index");
        }

    }
}