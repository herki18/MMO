using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using log4net.Core;
using MMO.Base.Api.V1;
using MMO.Data;
using MMO.Data.Entities;
using MMO.Web.Infrastructure;

namespace MMO.Web.Controllers.Api.V1
{
    [RoutePrefix("api/v1/launchers")]
    public class LauncherController : ApiController {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LauncherController));

        private readonly MMODatabseContext _database = new MMODatabseContext();
        [Route("latest"), HttpGet]
        public object GetLatestLauncher() {
            Log.Debug("GetLatestLauncher");
            var launcher = _database.Launchers.OrderByDescending(f => f.Version.Version).ThenByDescending(f => f.Version.Timestamp).FirstOrDefault();
            if (launcher == null)
            {
                return NotFound();
            }

            return new LatestLauncherResult(
                launcher.Version,
                new Uri(Request.RequestUri, Url.Route("Download", new { id = launcher.Id })).AbsoluteUri);
        }

        [Route("upload"), HttpPost, AuthorizeDeployToken]
        public async Task<IHttpActionResult> UploadLauncher() {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest();
            }

            return await UploadService.Upload(_database, Request, () => new Launcher());
        }
    }
}