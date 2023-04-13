namespace AutoTrust;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GithubPullRequestsSearch {
  [JsonPropertyName("total_count")]
  public long TotalCount { get; set; }
  //IncompleteResults is probably not that valuable:
  //https://docs.github.com/en/rest/search?apiVersion=2022-11-28#timeouts-and-incomplete-results
  [JsonPropertyName("incomplete_results")]
  public bool IncompleteResults { get; set; }

  public override string ToString() {
    var returnString = $"Pull requests: {this.TotalCount}\n";
    if (this.IncompleteResults) {
      returnString += "Warning: Was not able to fetch pull requests from Github!\n";
    }
    return returnString;
  }

  public static async Task<GithubPullRequestsSearch?> GetGithubPullRequestsSearch(HttpClient httpClient, string authorAndProject, string url, bool isDiagnostic) {
    try {
      // Fetch package data
      var githubPullRequestData = await httpClient.GetFromJsonAsync<GithubPullRequestsSearch>(url);
      if (isDiagnostic) {
        Console.WriteLine($"Found pull request data for {authorAndProject} from {url}");
      }
      return githubPullRequestData;
    }
    catch (HttpRequestException ex) {
      // Handle any exceptions thrown by the HTTP client.
      Console.WriteLine($"Error: An HTTP error occurred for {authorAndProject} from {url}: {ex.Message}");
    }
    catch (JsonException ex) {
      // Handle any exceptions thrown during JSON deserialization.
      Console.WriteLine($"Error: A JSON error occurred for {authorAndProject} from {url}: {ex.Message}");
    }
    return null;
  }

  // public static string GetOpenGithubPullRequestsUrl(string authorAndProject) => "https://api.github.com/search/issues?q=repo:" + authorAndProject + "+type:pr+state:open&per_page=1";
  public static string GetOpenGithubPullRequestsUrl(string authorAndProject) => "https://api.github.com/repos/" + authorAndProject + "/pulls?per_page=1&state=open";
  
  // public static string GetClosedGithubPullRequestsUrl(string authorAndProject) => "https://api.github.com/search/issues?q=repo:" + authorAndProject + "+type:pr+state:closed&per_page=1";
  public static string GetClosedGithubPullRequestsUrl(string authorAndProject) => "https://api.github.com/repos/" + authorAndProject + "/pulls?per_page=1&state=closed";

  public static string GetUpdatedGithubPullRequestsUrl(string authorAndProject, string lastUpdateTime) => $"https://api.github.com/search/issues?q=repo:{authorAndProject}+type:pr+state:open+updated:>{lastUpdateTime}&per_page=1";

}
