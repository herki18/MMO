using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMO.Data;
using MMO.Web.Areas.Admin.ViewModels;
using MMO.Web.Mailers;
using MMO.Web.ViewModels;

namespace MMO.Web.Controllers
{
    public class PasswordResetController : Controller
    {
        private readonly MMODatabseContext _database = new MMODatabseContext();
        public ActionResult Index(PasswordResetError? error) {

            return View(new PasswordResetIndex() {
                Error = error
            });
        }

        public ActionResult Index(PasswordResetIndex form) {
            if (!ModelState.IsValid) {
                return View(form);
            }

            var user = _database.Users.SingleOrDefault(t => t.Email == form.Email);

            if (user == null) {
                return RedirectToAction("confirm");
            }

            user.GenerateResetPasswordToken();

            _database.SaveChanges();

            new UserMailer().ResetPassword(user).Send();
            return RedirectToAction("confirm");
        }

        public ActionResult Confirm() {
            return View();
        }

        public ActionResult Reset(string token) {
            var user = _database.Users.SingleOrDefault(t => t.ResetPasswordToken == token);

            if (user == null) {
                return RedirectToAction("index", new {error=PasswordResetError.TokenNotFound});
            }

            if (user.ResetPasswordTokenExpiresAt < DateTime.UtcNow) {
                return RedirectToAction("index", new {error = PasswordResetError.TokeExpired});
            }

            return View(new PasswordResetReset {
                Username = user.UserName
            });
        }

        [HttpPost]
        public ActionResult Reset(string token, PasswordResetReset form) {
            var user = _database.Users.SingleOrDefault(t => t.ResetPasswordToken == token);

            if (user == null)
            {
                return RedirectToAction("index", new { error = PasswordResetError.TokenNotFound });
            }

            if (user.ResetPasswordTokenExpiresAt < DateTime.UtcNow)
            {
                return RedirectToAction("index", new { error = PasswordResetError.TokeExpired });
            }

            form.Username = user.UserName;

            if (!ModelState.IsValid) {
                return View(form);
            }

            user.SetPassword(form.NewPassword);
            user.ClearResetPasswordToken();
            _database.SaveChanges();

            Auth.User = user;
            return RedirectToAction("index", "home");
        }
    }
}