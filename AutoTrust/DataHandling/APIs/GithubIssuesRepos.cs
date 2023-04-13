namespace AutoTrust;
using System.Text.Json;
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

  public static async Task<int?> GetGithubIssues(HttpClient httpClient, string authorAndProject, string url, bool isDiagnostic) {
    try {
      // Fetch package data
      var res = await httpClient.GetAsync(url);
      var totalResponseCount = HelperFunctions.GetLastPageNumber(res.Headers.GetValues("Link").FirstOrDefault());

      if (isDiagnostic) {
        Console.WriteLine($"Found issue data for {authorAndProject} from {url}");
      }
      return totalResponseCount;
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

  public static string GetOpenGithubIssuesUrl(string authorAndProject) => "https://api.github.com/search/issues?q=repo:" + authorAndProject + "+type:issue+state:open&per_page=1";

  public static string GetClosedGithubIssuesUrl(string authorAndProject) => "https://api.github.com/search/issues?q=repo:" + authorAndProject + "+type:issue+state:closed&per_page=1";

}
