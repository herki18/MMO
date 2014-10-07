using System.Net.Http;
using System.Threading.Tasks;
using MMO.Data;
using MMO.Data.Entities;
using MMO.Web.Infrastructure;
using System.Web.Http;

namespace MMO.Web.Controllers.Api.V1
{
    [RoutePrefix("api/v1/clients")]
    public class ClientController : ApiController
    {
        private readonly MMODatabseContext _database = new MMODatabseContext();
        [Route("latest"), HttpGet]
        public object GetLatestClient() {
            return new {};
        }

        [Route("upload"), HttpPost, AuthorizeDeployToken]
        public  async Task<IHttpActionResult> UploadClient() {
            if (!Request.Content.IsMimeMultipartContent()) {
                return BadRequest();
            }

            return await UploadService.Upload(_database, Request, () => new Client());
        }
    }
}