using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using MMO.Data;
using MMO.Web.ViewModels;

namespace MMO.Web.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login() {
            return View(new AuthLogin());
        }

        [HttpPost]
        public ActionResult Login(AuthLogin form, string returnUrl) {
            if (!ModelState.IsValid) {
                return View(form);
            }

            using (var database = new MMODatabseContext()) {
                var user = database.Users.Include(t=>t.Roles).SingleOrDefault(t => t.UserName == form.UserName);
                if (user == null || !user.CheckPassword(form.Password)) {
                    ModelState.AddModelError("Password", "Username or password is incorrect");
                    return View(form);
                }

                Auth.User = user;
                if (!string.IsNullOrWhiteSpace(returnUrl)) {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("index", "home", new {area = ""});
            }
        }

        [HttpPost]
        [Authorize(Roles = "registered")]
        public ActionResult Logout() {
            Auth.LogOut();
            return RedirectToAction("login");
        }
    }
}