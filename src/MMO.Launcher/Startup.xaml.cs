using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows;
using Ionic.Zip;
using MMO.Base;
using MMO.Base.Api.V1;
using MMO.Base.Infrastructure;
using Newtonsoft.Json;

namespace MMO.Launcher
{
    public partial class Startup {

        public Startup()
        {
            InitializeComponent();
        }

        private async void CheckForUpdates(object sender, RoutedEventArgs e) {
            var arguments = Environment.GetCommandLineArgs();
            if (arguments.Length == 2) {
                var firstArgument = arguments[1];
                if (firstArgument.StartsWith("-temp=")) {
                    var tempDirectory = firstArgument.Substring("-temp=".Length);
                    Directory.Delete(tempDirectory, true);
                }
            }

            var currentVersion = BuildNumberAttribute.CurrentBuildNumber();
            if (currentVersion == null) {
                StatusLabel.Content = "Fatal error. Unable to establish current launcher version due to missing BuildInfo.";
                return;
            }

            var progressHandler = new ProgressMessageHandler(new HttpClientHandler());
            progressHandler.HttpReceiveProgress += HttpReceiveProgress;


            var httpClient = new HttpClient(progressHandler);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            StatusLabel.Content = "Querying for latest launcher...";

            var latestLauncher = JsonConvert.DeserializeObject<LatestLauncherResult>(await httpClient.GetStringAsync(
                string.Format("http://{0}/api/v1/launchers/latest", ConfigurationManager.AppSettings["WebApiDomain"])));

            if (currentVersion.Number.Version >= latestLauncher.Version.Version) {
                NavigateToMainWindow();
                return;
            }

            var tempZipFileName = Path.GetTempFileName();

            StatusLabel.Content = "Downloading Launcher...";
            using (var stream = await httpClient.GetStreamAsync(latestLauncher.DownloadUrl))
            {
                using (var fileStream = File.Open(tempZipFileName, FileMode.Open)) {
                    await stream.CopyToAsync(fileStream);
                }
            }
            string tempDirecotryName;
            using (var random = new RNGCryptoServiceProvider()) {
                var buffer = new byte[32];
                random.GetNonZeroBytes(buffer);
                tempDirecotryName = "temp-" + Convert.ToBase64String(buffer).Replace('=', '-').Replace('/', '-');
            }

            Directory.CreateDirectory(tempDirecotryName);

            using (var zip = ZipFile.Read(tempZipFileName)) {
                foreach (var entry in zip.Entries) {
                    if (File.Exists(entry.FileName)) {
                        File.Move(entry.FileName, Path.Combine(tempDirecotryName, entry.FileName));
                    }

                    entry.Extract(Directory.GetCurrentDirectory());
                }
            }

            File.Delete(tempZipFileName);

            Process.Start(Path.GetFileName(Assembly.GetEntryAssembly().Location), "-temp=" + tempDirecotryName);
            Close();
        }

        private void HttpReceiveProgress(object sender, HttpProgressEventArgs e) {
            Dispatcher.Invoke(() => {
                UpdateProgress.Value = e.ProgressPercentage;
            });
        }

        private void NavigateToMainWindow() {
            var mainWindow = new MainWindow();
            App.Current.MainWindow = mainWindow;
            Close();
            mainWindow.Show();
        }
    }
}
