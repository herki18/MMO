using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMO.Data;
using MMO.Data.Entities;
using MMO.Data.Services;
using MMO.Web.Areas.Admin.ViewModels;

namespace MMO.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        private readonly MMODatabseContext _database = new MMODatabseContext();
        // GET: Admin/Home
        public ActionResult Index() {
            var roles = _database.Roles.ToList();
            var settings = new MMOSettingService(_database);

            return View(new HomeIndex() {
                EnabledGameRoles = roles.Select(t=> new UserRole(t.Id,t.Name,settings.IsGameEnabledForRole(t))).ToList()
            });
        }

        [HttpPost]
        public ActionResult Index(HomeIndex form) {
            if (!ModelState.IsValid) {
                return View(form);
            }

            var roles = _database.Roles.ToList();
            var settings = new MMOSettingService(_database);

            var enabledRoles = new HashSet<Role>();
            foreach (var formRole in form.EnabledGameRoles) {
                if (!formRole.IsSeleceted) {
                    continue;
                }

                enabledRoles.Add(roles.Single(f => f.Id == formRole.Id));
            }
            settings.SetEnabledGameRoles(enabledRoles);
            return View(form);
        }
    }
}