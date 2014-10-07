using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MMO.Base.Api.V1;
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
            var client = _database.Clients.OrderByDescending(f => f.Version.Version).ThenByDescending(f => f.Version.Timestamp).FirstOrDefault();
            if (client == null) {
                return NotFound();
            }

            return new LatestClientResult(
                client.Version, 
                new Uri(Request.RequestUri, Url.Route("Download", new {id = client.Id})).AbsoluteUri);
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