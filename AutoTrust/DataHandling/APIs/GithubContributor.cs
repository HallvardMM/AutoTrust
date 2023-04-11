namespace AutoTrust;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GithubContributor {
  [JsonPropertyName("login")]
  public string Login { get; set; } = string.Empty;
  [JsonPropertyName("id")]
  public int Id { get; set; }
  [JsonPropertyName("node_id")]
  public string NodeId { get; set; } = string.Empty;
  [JsonPropertyName("avatar_url")]
  public string AvatarUrl { get; set; } = string.Empty;
  [JsonPropertyName("gravatar_id")]
  public string GravatarId { get; set; } = string.Empty;
  [JsonPropertyName("url")]
  public string Url { get; set; } = string.Empty;
  [JsonPropertyName("html_url")]
  public string HtmlUrl { get; set; } = string.Empty;
  [JsonPropertyName("followers_url")]
  public string FollowersUrl { get; set; } = string.Empty;
  [JsonPropertyName("following_url")]
  public string FollowingUrl { get; set; } = string.Empty;
  [JsonPropertyName("gists_url")]
  public string GistsUrl { get; set; } = string.Empty;
  [JsonPropertyName("starred_url")]
  public string StarredUrl { get; set; } = string.Empty;
  [JsonPropertyName("subscriptions_url")]
  public string SubscriptionsUrl { get; set; } = string.Empty;
  [JsonPropertyName("organizations_url")]
  public string OrganizationsUrl { get; set; } = string.Empty;
  [JsonPropertyName("repos_url")]
  public string ReposUrl { get; set; } = string.Empty;
  [JsonPropertyName("events_url")]
  public string EventsUrl { get; set; } = string.Empty;
  [JsonPropertyName("received_events_url")]
  public string ReceivedEventsUrl { get; set; } = string.Empty;
  [JsonPropertyName("type")]
  public string Type { get; set; } = string.Empty;
  [JsonPropertyName("site_admin")]
  public bool SiteAdmin { get; set; }
  [JsonPropertyName("contributions")]
  public int Contributions { get; set; }

  public static async Task<List<GithubContributor?>?> GetGithubContributors(HttpClient httpClient, string authorAndProject, bool isDiagnostic) {
    var url = $"https://api.github.com/repos/{authorAndProject}/contributors?per_page={Contributors.NumberOfContributorsThreshold}";
    try {
      // Fetch data from github
      // var contributors = new List<GithubContributor>();
      var contributors = await httpClient.GetFromJsonAsync<List<GithubContributor?>?>(url);
      if (isDiagnostic) {
        Console.WriteLine($"Found {contributors?.Count} contributors for {authorAndProject} in {url}");
      }
      return contributors;
    }
    catch (HttpRequestException ex) {
      // Handle any exceptions thrown by the HTTP client.
      if (isDiagnostic) {
        Console.WriteLine($"Error: An HTTP error occurred for {authorAndProject} from {url}: {ex.Message}");
      }
    }
    catch (JsonException ex) {
      // Handle any exceptions thrown during JSON deserialization.
      if (isDiagnostic) {
        Console.WriteLine($"Error: A JSON error occurred for {authorAndProject} from {url}: {ex.Message}");
      }
    }
    return null;
  }
}
