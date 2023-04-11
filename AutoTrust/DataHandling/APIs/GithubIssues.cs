namespace AutoTrust;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GithubIssues {
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

  public static async Task<GithubIssues?> GetGithubIssues(HttpClient httpClient, string authorAndProject, bool isDiagnostic) {
    var githubIssuesUrl = GetGithubIssuesUrl(authorAndProject);
    try {
      // Fetch package data
      var githubIssueData = await httpClient.GetFromJsonAsync<GithubIssues>(githubIssuesUrl);
      if (isDiagnostic) {
        Console.WriteLine($"Found issue data for {authorAndProject} from {githubIssuesUrl}");
      }
      return githubIssueData;
    }
    catch (HttpRequestException ex) {
      // Handle any exceptions thrown by the HTTP client.
      if (isDiagnostic) {
        Console.WriteLine($"Error: An HTTP error occurred for {authorAndProject} from {githubIssuesUrl}: {ex.Message}");
      }
    }
    catch (JsonException ex) {
      // Handle any exceptions thrown during JSON deserialization.
      if (isDiagnostic) {
        Console.WriteLine($"Error: A JSON error occurred for {authorAndProject} from {githubIssuesUrl}: {ex.Message}");
      }
    }
    return null;
  }

  public static string GetGithubIssuesUrl(string authorAndProject) => "https://api.github.com/search/issues?q=repo:" + authorAndProject + "+type:issue+state:open&per_page=1";
}
