namespace AutoTrust;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GithubPullRequestsRepos {
  [JsonPropertyName("url")]
  public long Url { get; set; }

  public static async Task<int?> GetGithubPullRequestsRepos(HttpClient httpClient, string authorAndProject, string url, bool isDiagnostic) {
    try {
      // Fetch package data
      var res = await httpClient.GetAsync(url);
      var totalResponseCount = HelperFunctions.GetLastPageNumber(res.Headers.GetValues("Link").FirstOrDefault());

      if (isDiagnostic) {
        Console.WriteLine($"Found pull request data for {authorAndProject} from {url}");
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

  public static string GetOpenGithubPullRequestsUrl(string authorAndProject) => "https://api.github.com/repos/" + authorAndProject + "/pulls?per_page=1&state=open";  
  public static string GetClosedGithubPullRequestsUrl(string authorAndProject) => "https://api.github.com/repos/" + authorAndProject + "/pulls?per_page=1&state=closed";


}
