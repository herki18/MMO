using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Results;
using MMO.Base;
using MMO.Base.Infrastructure;
using MMO.Base.Infrastructure.Extensions;
using MMO.Data;
using MMO.Data.Entities;

namespace MMO.Web.Infrastructure
{
    public static class UploadService
    {

        public static async Task<IHttpActionResult> Upload(MMODatabseContext database, HttpRequestMessage request, Func<Upload> uploadFactory) {
            var tempFileName = Path.GetTempFileName();
            var provider = new MultipartDataProvider(
                name => name.Equals("upload", StringComparison.OrdinalIgnoreCase) ? File.Open(tempFileName, FileMode.Open) : null);
            await request.Content.ReadAsMultipartAsync(provider);

            if (!provider.IsValid) {
                return new BadRequestResult(request);
            }

            int timeStamp;
            short version;
            if (!provider.TryGetFormdata("timestamp", out timeStamp) || !provider.TryGetFormdata("version", out version)) {
                return new BadRequestResult(request);
            }

            if (provider.Files.Count() != 1) {
                return new BadRequestResult(request);
            }

            var file = provider.Files.Single();
            var upload = uploadFactory();
            upload.UploadedAt = DateTime.UtcNow;
            upload.Version = new BuildNumber(version, timeStamp);
            upload.OriginalFileName = file.Headers.ContentDisposition.FileName.TrimDoubleQuotes();
            upload.FileSizeBytes = (new FileInfo(tempFileName)).Length;

            database.Uploads.Add(upload);
            database.SaveChanges();

            var newFileName = Path.Combine(HostingEnvironment.MapPath("~/uploads"), upload.Id.ToString());
            File.Move(tempFileName, newFileName);

            return new OkResult(request);
        }
    }
} 