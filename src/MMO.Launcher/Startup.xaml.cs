using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using MMO.Base;
using MMO.Base.Api.V1;
using Newtonsoft.Json;

namespace MMO.Launcher
{
    public partial class Startup {

        public Startup()
        {
            InitializeComponent();
        }

        private async void CheckForUpdates(object sender, RoutedEventArgs e) {
            var currentVersion = BuildNumberAttribute.CurrentBuildNumber();
            if (currentVersion == null) {
                StatusLabel.Content = "Fatal error. Unable to establish current launcher version due to missing BuildInfo.";
                return;
            }

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            StatusLabel.Content = "Querying for latest launcher...";

            var latestLauncher = JsonConvert.DeserializeObject<LatestLauncherResult>(await httpClient.GetStringAsync(
                string.Format("http://{0}/api/v1/launchers/latest", ConfigurationManager.AppSettings["WebApiDomain"])));

            StatusLabel.Content = latestLauncher.Version.ToString();
        }
    }
}
