using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Reflection;

namespace UiBot
{
    internal class UpdateCheck
    {
        public async Task CheckForUpdatesAsync()
        {
            string repo = "sprollucy/Tarkov-Twitch-Bot-Working"; // GitHub repository details

            string localVersion = GetLocalVersion();
            if (string.IsNullOrEmpty(localVersion))
            {
                MessageBox.Show("Could not retrieve the local version.", "Update Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var latestRelease = await CheckGitHubUpdate(repo);

            if (latestRelease != null)
            {
                string latestVersion = latestRelease["tag_name"].ToString();
                if (IsNewerVersionAvailable(localVersion, latestVersion))
                {
                    DialogResult result = MessageBox.Show($"A new version ({latestVersion}) is available. Do you want to check it out?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        string url = latestRelease["html_url"].ToString();
                        try
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = url,
                                UseShellExecute = true
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error opening URL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("You have the latest version installed.", "Update Checker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Could not fetch the latest version details.", "Update Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task ButtonCheckForUpdatesAsync()
        {
            string repo = "sprollucy/Tarkov-Twitch-Bot-Working"; // GitHub repository details

            string localVersion = GetLocalVersion();
            if (string.IsNullOrEmpty(localVersion))
            {
                MessageBox.Show("Could not retrieve the local version.", "Update Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var latestRelease = await CheckGitHubUpdate(repo);

            if (latestRelease != null)
            {
                string latestVersion = latestRelease["tag_name"].ToString();
                if (IsNewerVersionAvailable(localVersion, latestVersion))
                {
                    DialogResult result = MessageBox.Show($"A new version ({latestVersion}) is available. Do you want to check it out?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        string url = latestRelease["html_url"].ToString();
                        try
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = url,
                                UseShellExecute = true
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error opening URL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You have the latest version installed.", "Update Checker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Could not fetch the latest version details.", "Update Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetLocalVersion()
        {
            // Get the version number of the current executing assembly
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return version?.ToString();
        }
        private bool IsNewerVersionAvailable(string currentVersion, string latestVersion)
        {
            Version current = new Version(currentVersion);
            Version latest = new Version(latestVersion);
            return latest > current;
        }
        private async Task<JObject> CheckGitHubUpdate(string repo)
        {
            string url = $"https://api.github.com/repos/{repo}/releases/latest";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("CSharpApp");
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JObject.Parse(jsonResponse);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
