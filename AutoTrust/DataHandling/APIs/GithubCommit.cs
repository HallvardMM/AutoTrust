namespace AutoTrust;

public class GithubCommit {
  [JsonPropertyName("sha")]
  public string Sha { get; set; }
  [JsonPropertyName("node_id")]
  public string NodeId { get; set; }
  [JsonPropertyName("commit")]
  public GithubCommitDetails Commit { get; set; }
  [JsonPropertyName("url")]
  public string Url { get; set; }
  [JsonPropertyName("html_url")]
  public string HtmlUrl { get; set; }
  [JsonPropertyName("comments_url")]
  public string CommentsUrl { get; set; }
  [JsonPropertyName("author")]
  public GithubPersonFullInfo Author { get; set; }
  [JsonPropertyName("committer")]
  public GithubPersonFullInfo Committer { get; set; }
  [JsonPropertyName("parents")]
  public GithubCommitParent[] Parents { get; set; }

  public static async Task<List<GithubCommit?>?> GetGithubCommit(HttpClient httpClient, string authorAndProject, bool isDiagnostic) {
    var fetchCommitNumber = 100;
    var url = $"https://api.github.com/repos/{authorAndProject}/commits?since=2022-04-11&per_page={fetchCommitNumber}";
  }
}

public class GithubCommitDetails {
  [JsonPropertyName("message")]
  public string Message { get; set; }
  [JsonPropertyName("author")]
  public GithubCommitPerson Author { get; set; }
  [JsonPropertyName("committer")]
  public GithubCommitPerson Committer { get; set; }
  [JsonPropertyName("tree")]
  public GithubCommitTree Tree { get; set; }
  [JsonPropertyName("url")]
  public string Url { get; set; }
  [JsonPropertyName("comment_count")]
  public int CommentCount { get; set; }
  [JsonPropertyName("verification")]
  public GithubCommitVerification Verification { get; set; }
}

public class GithubCommitPerson {
  [JsonPropertyName("name")]
  public string Name { get; set; }
  [JsonPropertyName("email")]
  public string Email { get; set; }
  [JsonPropertyName("date")]
  public DateTime Date { get; set; }
}

public class GithubCommitTree {
  [JsonPropertyName("sha")]
  public string Sha { get; set; }
  [JsonPropertyName("url")]
  public string Url { get; set; }
}

public class GithubCommitParent {
  [JsonPropertyName("sha")]
  public string Sha { get; set; }
  [JsonPropertyName("url")]
  public string Url { get; set; }
  [JsonPropertyName("html_url")]
  public string HtmlUrl { get; set; }
}

public class GithubCommitVerification {
  [JsonPropertyName("verified")]
  public bool Verified { get; set; }
  [JsonPropertyName("reason")]
  public string Reason { get; set; }
  [JsonPropertyName("signature")]
  public string Signature { get; set; }
  [JsonPropertyName("payload")]
  public string Payload { get; set; }
}

public class GithubPersonFullInfo {
  [JsonPropertyName("login")]
  public string Login { get; set; }
  [JsonPropertyName("id")]
  public int Id { get; set; }
  [JsonPropertyName("node_id")]
  public string NodeId { get; set; }
  [JsonPropertyName("avatar_url")]
  public string AvatarUrl { get; set; }
  [JsonPropertyName("gravatar_id")]
  public string GravatarId { get; set; }
  [JsonPropertyName("url")]
  public string Url { get; set; }
  [JsonPropertyName("html_url")]
  public string HtmlUrl { get; set; }
  [JsonPropertyName("followers_url")]
  public string FollowersUrl { get; set; }
  [JsonPropertyName("following_url")]
  public string FollowingUrl { get; set; }
  [JsonPropertyName("gists_url")]
  public string GistsUrl { get; set; }
  [JsonPropertyName("starred_url")]
  public string StarredUrl { get; set; }
  [JsonPropertyName("subscriptions_url")]
  public string SubscriptionsUrl { get; set; }
  [JsonPropertyName("organizations_url")]
  public string OrganizationsUrl { get; set; }
  [JsonPropertyName("repos_url")]
  public string ReposUrl { get; set; }
  [JsonPropertyName("events_url")]
  public string EventsUrl { get; set; }
  [JsonPropertyName("received_events_url")]
  public string ReceivedEventsUrl { get; set; }
  [JsonPropertyName("type")]
  public string Type { get; set; }
  [JsonPropertyName("site_admin")]
  public bool SiteAdmin { get; set; }
}