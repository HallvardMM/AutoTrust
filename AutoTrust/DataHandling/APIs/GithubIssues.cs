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

  public static async Task<GithubIssues?> GetGithubIssues(HttpClient httpClient, string repositoryUrl) {
    try {
      // Fetch package data
      var githubIssuesUrl = GetGithubIssuesUrl(repositoryUrl);
      httpClient.DefaultRequestHeaders.Add("User-Agent", "request");
      var githubIssueData = await httpClient.GetFromJsonAsync<GithubIssues>(githubIssuesUrl);
      return githubIssueData;
    }
    catch (HttpRequestException ex) {
      // Handle any exceptions thrown by the HTTP client.
      Console.WriteLine($"An HTTP error occurred: {ex.Message}");
    }
    catch (JsonException ex) {
      // Handle any exceptions thrown during JSON deserialization.
      Console.WriteLine($"A JSON error occurred: {ex.Message}");
    }
    return null;
  }

  public static string GetGithubIssuesUrl(string repositoryUrl) {
    var githubApiUrl = GithubPackage.GetGithubApiUrl(repositoryUrl);
    return githubApiUrl.Replace("repos/", "search/issues?q=repo:") + "+type:issue+state:open&per_page=1";
  }
}
