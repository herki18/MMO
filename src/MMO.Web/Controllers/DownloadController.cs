using System.IO;
using System.Net.Mime;
using System.Web.Mvc;
using MMO.Data;

namespace MMO.Web.Controllers
{
    public class DownloadController : Controller
    {
        private readonly MMODatabseContext _database = new MMODatabseContext();
        public ActionResult Download(int id) {
            var upload = _database.Uploads.Find(id);

            if (upload == null) {
                return HttpNotFound();
            }

            var uploadFilePath = Path.Combine(Server.MapPath("~/uploads"), upload.Id.ToString());

            if (!System.IO.File.Exists(uploadFilePath)) {
                return HttpNotFound();
            }

            return File(uploadFilePath, MediaTypeNames.Application.Octet, upload.OriginalFileName);
        }
    }
}