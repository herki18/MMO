using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMO.Data;
using MMO.Data.Entities;
using MMO.Web.Areas.Admin.ViewModels;

namespace MMO.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly MMODatabseContext _databse = new MMODatabseContext();

        // GET: Admin/User
        public ActionResult Index()
        {
            return View(new UserIndex {
                Users = _databse.Users.Include(t=>t.Roles).ToList()
            });
        }

        public ActionResult Create() {
            return View(new UserCreate {
                Roles = _databse.Roles.AsEnumerable().Select(t=> new UserRole(t.Id,t.Name,false)).ToList()
            });
        }

        [HttpPost]
        public ActionResult Create(UserCreate from) {
            if (_databse.Users.Any(t => t.UserName == from.UserName)) {
                ModelState.AddModelError("Username", "Usernames must be unique");
            }
            if (_databse.Users.Any(t=>t.Email == from.Email)) {
                ModelState.AddModelError("Email", "Emails must be unique");
            }
            if (!ModelState.IsValid) {
                return View(from);
            }

            var user = new User {
                Email = from.Email,
                UserName = from.Password,
                Roles = new List<Role>()
            };

            user.SetPassword(from.Password);
            SyncRoles(user.Roles, from.Roles);

            _databse.Users.Add(user);
            _databse.SaveChanges();

            return RedirectToAction("index");
        }

        public ActionResult Edit(int id) {
            var user = _databse.Users.Include(t=>t.Roles).SingleOrDefault(t=>t.Id == id);

            if (user == null) {
                return RedirectToAction("index");
            }

            return View(new UserEdit {
                Email = user.Email,
                UserName = user.UserName,
                Roles = _databse.Roles.AsEnumerable().Select(t => new UserRole(t.Id, t.Name, user.Roles.Contains(t))).ToList()
            });

            
        }

        [HttpPost]
        public ActionResult Edit(int id, UserEdit form) {
            var user = _databse.Users.Include(t => t.Roles).Single(t => t.Id == id);

            if (_databse.Users.Any(t => t.UserName == form.UserName && t.Id != id))
            {
                ModelState.AddModelError("Username", "Usernames must be unique");
            }
            if (_databse.Users.Any(t => t.Email == form.Email && t.Id != id))
            {
                ModelState.AddModelError("Email", "Emails must be unique");
            }
            if (user.UserName == "admin" && form.UserName != user.UserName) {
                ModelState.AddModelError("Username", "Cannot change admin username");
            }
            if (Auth.User.Id == id && !form.Roles.Any(t => t.Name == "admin" && t.IsSeleceted)) {
                ModelState.AddModelError("Username", "Cannot remove admin role from yourself!");
            }
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            

            user.Email = form.Email;
            user.UserName = form.UserName;

            SyncRoles(user.Roles, form.Roles);

            //_databse.Users.Add(user);
            _databse.SaveChanges();

            return RedirectToAction("index");
        }

        public ActionResult ResetPassword(int id) {
            var user = _databse.Users.Find(id);
            if (user == null) {
                return RedirectToAction("index");
            }

            return View(new UserResetPassword {
                UserName = user.UserName
            });
        }

        [HttpPost]
        public ActionResult ResetPassword(int id, UserResetPassword form)
        {
            var user = _databse.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("index");
            }

            form.UserName = user.UserName;

            if (!ModelState.IsValid) {
                return View(form);
            }

            user.SetPassword(form.Password);
            _databse.SaveChanges();

            return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            var user = _databse.Users.Find(id);
            if (user == null) {
                return RedirectToAction("index");
            }

            if (user.Id == Auth.User.Id) {
                return RedirectToAction("index");
            }

            _databse.Users.Remove(user);
            _databse.SaveChanges();

            return RedirectToAction("index");
        }

        private void SyncRoles(ICollection<Role> entityRoles, IEnumerable<UserRole> formRoles) {
            entityRoles.Clear();
            foreach (var role in formRoles.Where(t=>t.IsSeleceted)) {
                entityRoles.Add(_databse.Roles.Find(role.Id));
            }
        }
    }
}