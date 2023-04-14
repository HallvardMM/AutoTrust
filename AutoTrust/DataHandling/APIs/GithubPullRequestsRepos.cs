namespace AutoTrust;
using System.Text.Json.Serialization;

public class GithubPullRequestsRepos {
  [JsonPropertyName("url")]
  public long Url { get; set; }

  public static async Task<int?> GetGithubPullRequestsRepos(HttpClient httpClient, string? githubToken, string authorAndProject, string url, bool isDiagnostic) => await DataHandler.FetchGithubHeaderCount(httpClient, githubToken, url, authorAndProject, isDiagnostic,
    $"Found pull request data for {authorAndProject} from {url}");

  public static string GetOpenGithubPullRequestsUrl(string authorAndProject) => "https://api.github.com/repos/" + authorAndProject + "/pulls?per_page=1&state=open";
  public static string GetClosedGithubPullRequestsUrl(string authorAndProject) => "https://api.github.com/repos/" + authorAndProject + "/pulls?per_page=1&state=closed";


}
