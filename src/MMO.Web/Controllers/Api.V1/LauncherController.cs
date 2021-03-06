﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MMO.Base.Api.V1;
using MMO.Data;
using MMO.Data.Entities;
using MMO.Web.Infrastructure;
using Serilog;

namespace MMO.Web.Controllers.Api.V1
{
    [RoutePrefix("api/v1/launchers")]
    public class LauncherController : ApiController {
        //private static readonly Serilog.ILogger Log = Serilog.Log.ForContext<LauncherController>();

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