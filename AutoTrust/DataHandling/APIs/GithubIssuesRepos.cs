namespace AutoTrust;
using System.Text.Json.Serialization;

public class GithubIssuesRepos {
  [JsonPropertyName("total_count")]
  public long TotalCount { get; set; }
  //IncompleteResults is probably not that valuable:
  //https://docs.github.com/en/rest/search?apiVersion=2022-11-28#timeouts-and-incomplete-results
  [JsonPropertyName("incomplete_results")]
  public bool IncompleteResults { get; set; }

  public override string ToString() {
    var returnString = $"Open issues: {this.TotalCount}\n";
    if (this.IncompleteResults) {
      returnString += "Warning: Was not able to fetch all open issues from Github!\n";
    }
    return returnString;
  }

  public static async Task<int?> GetGithubIssues(HttpClient httpClient, string? githubToken, string authorAndProject, string url, bool isDiagnostic) => await DataHandler.FetchGithubHeaderCount(httpClient, githubToken, url, authorAndProject, isDiagnostic,
    $"Found issue data for {authorAndProject} from {url}");

  public static string GetOpenGithubIssuesUrl(string authorAndProject) => "https://api.github.com/search/issues?q=repo:" + authorAndProject + "+type:issue+state:open&per_page=1";

  public static string GetClosedGithubIssuesUrl(string authorAndProject) => "https://api.github.com/search/issues?q=repo:" + authorAndProject + "+type:issue+state:closed&per_page=1";

}
