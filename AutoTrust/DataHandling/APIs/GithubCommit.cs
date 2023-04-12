//REST API Documentation for "List commits": 
// https://docs.github.com/en/rest/commits/commits?apiVersion=2022-11-28

namespace AutoTrust;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public class GithubCommit {
  [JsonPropertyName("sha")]
  public string Sha { get; set; } = string.Empty;
  [JsonPropertyName("node_id")]
  public string NodeId { get; set; } = string.Empty;
  [JsonPropertyName("commit")]
  public GithubCommitDetails? Commit { get; set; }
  [JsonPropertyName("url")]
  public string Url { get; set; } = string.Empty;
  [JsonPropertyName("html_url")]
  public string HtmlUrl { get; set; } = string.Empty;
  [JsonPropertyName("comments_url")]
  public string CommentsUrl { get; set; } = string.Empty;
  [JsonPropertyName("author")]
  public GithubPersonFullInfo? Author { get; set; }
  [JsonPropertyName("committer")]
  public GithubPersonFullInfo? Committer { get; set; }
  [JsonPropertyName("parents")]
  public GithubCommitParent[]? Parents { get; set; }

  public static async Task<List<GithubCommit?>?> GetGithubCommits(HttpClient httpClient, string authorAndProject, bool isDiagnostic) {
    var fetchCommitNumber = 100;
    var oneYearAgoString = DateTime.UtcNow.AddYears(-1).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
    var commitsUrl = $"https://api.github.com/repos/{authorAndProject}/commits?since={oneYearAgoString}&per_page={fetchCommitNumber}";

    try {
      // Fetch data from github
      var commits = await httpClient.GetFromJsonAsync<List<GithubCommit?>?>(commitsUrl);
      return commits;
    }
    catch (HttpRequestException ex) {
      // Handle any exceptions thrown by the HTTP client.
      if (isDiagnostic) {
        Console.WriteLine($"Error: An HTTP error occurred for {authorAndProject} from {commitsUrl}: {ex.Message}");
      }
    }
    catch (JsonException ex) {
      // Handle any exceptions thrown during JSON deserialization.
      if (isDiagnostic) {
        Console.WriteLine($"Error: A JSON error occurred for {authorAndProject} from {commitsUrl}: {ex.Message}");
      }
    }
    return null;
  }
}

public class GithubCommitDetails {
  [JsonPropertyName("message")]
  public string Message { get; set; } = string.Empty;
  [JsonPropertyName("author")]
  public GithubCommitPerson? Author { get; set; }
  [JsonPropertyName("committer")]
  public GithubCommitPerson? Committer { get; set; }
  [JsonPropertyName("tree")]
  public GithubCommitTree? Tree { get; set; }
  [JsonPropertyName("url")]
  public string Url { get; set; } = string.Empty;
  [JsonPropertyName("comment_count")]
  public int CommentCount { get; set; }
  [JsonPropertyName("verification")]
  public GithubCommitVerification? Verification { get; set; }
}

public class GithubCommitPerson {
  [JsonPropertyName("name")]
  public string Name { get; set; } = string.Empty;
  [JsonPropertyName("email")]
  public string Email { get; set; } = string.Empty;
  [JsonPropertyName("date")]
  public DateTime Date { get; set; }
}

public class GithubCommitTree {
  [JsonPropertyName("sha")]
  public string Sha { get; set; } = string.Empty;
  [JsonPropertyName("url")]
  public string Url { get; set; } = string.Empty;
}

public class GithubCommitParent {
  [JsonPropertyName("sha")]
  public string Sha { get; set; } = string.Empty;
  [JsonPropertyName("url")]
  public string Url { get; set; } = string.Empty;
  [JsonPropertyName("html_url")]
  public string HtmlUrl { get; set; } = string.Empty;
}

public class GithubCommitVerification {
  [JsonPropertyName("verified")]
  public bool Verified { get; set; }
  [JsonPropertyName("reason")]
  public string Reason { get; set; } = string.Empty;
  [JsonPropertyName("signature")]
  public string Signature { get; set; } = string.Empty;
  [JsonPropertyName("payload")]
  public string Payload { get; set; } = string.Empty;
}

public class GithubPersonFullInfo {
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
}
