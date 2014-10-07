using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MMO.Data;
using MMO.Data.Entities;
using MMO.Web.Infrastructure;

namespace MMO.Web.Controllers.Api.V1
{
    [System.Web.Mvc.RoutePrefix("api/v1/launchers")]
    public class LauncherController : ApiController
    {
        private readonly MMODatabseContext _database = new MMODatabseContext();
        [System.Web.Mvc.Route("latest"), System.Web.Mvc.HttpGet]
        public object GetLatestLauncher() {
            return new {
                Version = "5",
                Url = "blegh"
            };
        }

        [System.Web.Mvc.Route("upload"), System.Web.Mvc.HttpPost, AuthorizeDeployToken]
        public async Task<IHttpActionResult> UploadLauncher() {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return BadRequest();
            }

            return await UploadService.Upload(_database, Request, () => new Client());
        }
    }
}