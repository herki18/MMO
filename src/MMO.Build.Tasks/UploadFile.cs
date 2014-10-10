using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Build.Utilities;

namespace MMO.Build.Tasks
{
    public class UploadFile : Task
    {
        public string Type { get; set; }
        public string Env { get; set; }
        public string Domain { get; set; }
        public string DeployToken { get; set; }
        public string File { get; set; }
        public string Timestamp { get; set; }
        public string VersionNumber { get; set; }



        public override bool Execute() {
            string apiEndPoint, filename;
            if (Type == "Launcher") {
                apiEndPoint = "Launchers";
                filename = "Launcher.zip";
            }
            else if (Type == "Client") {
                apiEndPoint = "Clients";
                filename = "Client.zip";
            }
            else {
                throw new ArgumentException();
            }

            var multiPartData = new MultipartFormDataContent();
            multiPartData.Add(new StringContent(Timestamp), "timestamp");
            multiPartData.Add(new StringContent(VersionNumber), "version");

            var fileContent = new StreamContent(System.IO.File.Open(File, FileMode.Open));
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") {
                FileName = filename,
                Name = "upload"
            };

            multiPartData.Add(fileContent);
            multiPartData.Headers.Add("deploy-token", DeployToken);

            var client = new HttpClient();
            var response = client.PostAsync(string.Format("http://{0}/api/v1/{1}/upload", Domain, apiEndPoint),
                multiPartData).Result;

            if (!response.IsSuccessStatusCode) {
                Log.LogError("Error upload file: {0}, {1}, {2}",response.StatusCode, response.Content.ToString(), string.Format("http://{0}/api/v1/{1}/upload", Domain, apiEndPoint));
            }

            return response.IsSuccessStatusCode;
        }
    }
}
