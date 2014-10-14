using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading.Tasks;
using System.Windows;
using Ionic.Zip;
using MMO.Base.Api.V1;
using Newtonsoft.Json;

namespace MMO.Launcher
{
    public partial class MainWindow {
        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = _viewModel = new MainWindowViewModel();
            _viewModel.CanLogin = true;
        }

        private async void Login(object sender, RoutedEventArgs e) {
            _viewModel.CanLogin = false;
            var progressHandler = new ProgressMessageHandler(new HttpClientHandler());
            progressHandler.HttpReceiveProgress += HttpReceiveProgress;

            var httpClient = new HttpClient(progressHandler);

            var authResponse = await HttpPost<AuthValidateResponse, AuthValidateRequest>(httpClient,
                "api/v1/authentication/validate",
                new AuthValidateRequest(_viewModel.Username, Password.Password));

            if (!authResponse.CredentailsAreValid) {
                _viewModel.Status = "Invalid Username or Password";
                _viewModel.CanLogin = true;
                return;
            }

            var launcherData = new LauncherData();

            if (File.Exists("launcher.data")) {
                try {
                    launcherData = JsonConvert.DeserializeObject<LauncherData>(File.ReadAllText("launcher.data"));
                }
                catch (Exception) {
                }
            }

            _viewModel.Status = "Quering for latest client";

            var latestClient = JsonConvert.DeserializeObject<LatestClientResult>(await httpClient.GetStringAsync(
                string.Format("http://{0}/api/v1/clients/latest", ConfigurationManager.AppSettings["WebApiDomain"])));

            if (launcherData.CurrentClientVersion >= latestClient.Version.Version){
                await LaunchClient(httpClient);
                return;
            }

            _viewModel.Status = "Downloading latest client";

            using (var stream = await httpClient.GetStreamAsync(latestClient.DownloadUrl))
            {
                using (var fileStream = File.Open("client.tmp", FileMode.Create))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }

            if (Directory.Exists("Client")) {
                Directory.Delete("Client", true);
            }

            Directory.CreateDirectory("Client");

            _viewModel.Status = "Extracting client";
            await Task.Run(() => {
                using (var zip = ZipFile.Read("client.tmp")) {
                    var totalEntries = zip.Entries.Count;
                    var currentEntry = 0;

                    foreach (var entry in zip.Entries) {
                        entry.Extract("Client");
                        Dispatcher.Invoke(() => {
                            _viewModel.Precent = (double) currentEntry/totalEntries*100; }
                            );
                    }
                }
                File.Delete("client.tmp");
            });
            
            launcherData.CurrentClientVersion = latestClient.Version.Version;
            File.WriteAllText("launcher.data",JsonConvert.SerializeObject(launcherData));

            await LaunchClient(httpClient);
        }

        private async Task LaunchClient(HttpClient client) {
            _viewModel.Status = "Launching Client";

            var authResponse = await HttpPost<AuthGenerateTokenResponse, AuthGenerateTokenRequest>(
                client,
                "api/v1/authentication/login",
                new AuthGenerateTokenRequest(_viewModel.Username, Password.Password));

            if (!authResponse.CredentailsAreValid) {
                _viewModel.Status = "Invalid Username or Password.";
                _viewModel.CanLogin = true;
                return;
            }

            Process.Start(Path.Combine("Client", "MMO.exe"), string.Format("-token={0}", authResponse.Token));
            Close();
        }

        private async Task<TResponse> HttpPost<TResponse, TRequest>(HttpClient client, string apiEndPoint, TRequest parameters) {
            var response = await client.PostAsJsonAsync(
            string.Format("http://{0}/{1}", ConfigurationManager.AppSettings["WebApiDomain"], apiEndPoint), parameters);

            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }

        private void HttpReceiveProgress(object sender, HttpProgressEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                _viewModel.Precent = e.ProgressPercentage;
            });
        }
    }
}
