using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

namespace AutoTrust
{
  public class GithubIssues
  {
    [JsonPropertyName("total_count")]
    public long TotalCount { get; set; }
    //IncompleteResults is probably not that valuable:
    //https://docs.github.com/en/rest/search?apiVersion=2022-11-28#timeouts-and-incomplete-results
    [JsonPropertyName("incomplete_results")]
    public bool IncompleteResults { get; set; } 

    public override string ToString()
    {
      string returnString = $"Open issues: {TotalCount}\n";
      if (IncompleteResults)
      {
        returnString += "Warning: Was not able to fetch all open issues from Github!\n";
      }
      return returnString;
    }

    public async static Task<GithubIssues?> GetGithubIssues(HttpClient httpClient, string repositoryUrl)
    {
      try
      {
        // Fetch package data
        var githubIssuesUrl = GetGithubIssuesUrl(repositoryUrl);
        httpClient.DefaultRequestHeaders.Add("User-Agent", "request");
        GithubIssues? githubIssueData = await httpClient.GetFromJsonAsync<GithubIssues>(githubIssuesUrl);
        return githubIssueData;
      }
      catch (HttpRequestException ex)
      {
        // Handle any exceptions thrown by the HTTP client.
        Console.WriteLine($"An HTTP error occurred: {ex.Message}");
      }
      catch (JsonException ex)
      {
        // Handle any exceptions thrown during JSON deserialization.
        Console.WriteLine($"A JSON error occurred: {ex.Message}");
      }
      return null;
    }

    public static string GetGithubIssuesUrl(string RepositoryUrl)
    {
      string githubApiUrl = GithubPackage.GetGithubApiUrl(RepositoryUrl);
      return githubApiUrl.Replace("repos/", "search/issues?q=repo:") + "+type:issue+state:open&per_page=1";
    }
  }
}
