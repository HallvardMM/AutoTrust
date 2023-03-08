using System.Text.Json.Serialization;

// REST API Documentation for "Get a repository": 
// https://docs.github.com/en/rest/repos/repos?apiVersion=2022-11-28#get-a-repository

namespace AutoTrust
{
  public class GithubPackage
  {
    public long Id { get; set; }
    public string NodeId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = string.Empty;
    public bool Private { get; set; }
    public required GithubOwner Owner { get; set; }
    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Fork { get; set; }
    public string Url { get; set; } = string.Empty;
    [JsonPropertyName("forks_url")]
    public string ForksUrl { get; set; } = string.Empty;
    [JsonPropertyName("keys_url")]
    public string KeysUrl { get; set; } = string.Empty;
    [JsonPropertyName("collaborators_url")]
    public string CollaboratorsUrl { get; set; } = string.Empty;
    [JsonPropertyName("teams_url")]
    public string TeamsUrl { get; set; } = string.Empty;
    [JsonPropertyName("hooks_url")]
    public string HooksUrl { get; set; } = string.Empty;
    [JsonPropertyName("issue_events_url")]
    public string IssueEventsUrl { get; set; } = string.Empty;
    [JsonPropertyName("events_url")]
    public string EventsUrl { get; set; } = string.Empty;
    [JsonPropertyName("assignees_url")]
    public string AssigneesUrl { get; set; } = string.Empty;
    [JsonPropertyName("branches_url")]
    public string BranchesUrl { get; set; } = string.Empty;
    [JsonPropertyName("tags_url")]
    public string TagsUrl { get; set; } = string.Empty;
    [JsonPropertyName("blobs_url")]
    public string BlobsUrl { get; set; } = string.Empty;
    [JsonPropertyName("git_tags_url")]
    public string GitTagsUrl { get; set; } = string.Empty;
    [JsonPropertyName("git_refs_url")]
    public string GitRefsUrl { get; set; } = string.Empty;
    [JsonPropertyName("trees_url")]
    public string TreesUrl { get; set; } = string.Empty;
    [JsonPropertyName("statuses_url")]
    public string StatusesUrl { get; set; } = string.Empty;
    [JsonPropertyName("languages_url")]
    public string LanguagesUrl { get; set; } = string.Empty;
    [JsonPropertyName("stargazers_url")]
    public string StargazersUrl { get; set; } = string.Empty;
    [JsonPropertyName("contributors_url")]
    public string ContributorsUrl { get; set; } = string.Empty;
    [JsonPropertyName("subscribers_url")]
    public string SubscribersUrl { get; set; } = string.Empty;
    [JsonPropertyName("subscription_url")]
    public string SubscriptionUrl { get; set; } = string.Empty;
    [JsonPropertyName("commits_url")]
    public string CommitsUrl { get; set; } = string.Empty;
    [JsonPropertyName("git_commits_url")]
    public string GitCommitsUrl { get; set; } = string.Empty;
    [JsonPropertyName("comments_url")]
    public string CommentsUrl { get; set; } = string.Empty;
    [JsonPropertyName("issue_comment_url")]
    public string IssueCommentUrl { get; set; } = string.Empty;
    [JsonPropertyName("contents_url")]
    public string ContentsUrl { get; set; } = string.Empty;
    [JsonPropertyName("compare_url")]
    public string CompareUrl { get; set; } = string.Empty;
    [JsonPropertyName("merges_url")]
    public string MergesUrl { get; set; } = string.Empty;
    [JsonPropertyName("archive_url")]
    public string ArchiveUrl { get; set; } = string.Empty;
    [JsonPropertyName("downloads_url")]
    public string DownloadsUrl { get; set; } = string.Empty;
    [JsonPropertyName("Issues_url")]
    public string IssuesUrl { get; set; } = string.Empty;
    [JsonPropertyName("pulls_url")]
    public string PullsUrl { get; set; } = string.Empty;
    [JsonPropertyName("milestones_url")]
    public string MilestonesUrl { get; set; } = string.Empty;
    [JsonPropertyName("notifications_url")]
    public string NotificationsUrl { get; set; } = string.Empty;
    [JsonPropertyName("labels_url")]
    public string LabelsUrl { get; set; } = string.Empty;
    [JsonPropertyName("releases_url")]
    public string ReleasesUrl { get; set; } = string.Empty;
    [JsonPropertyName("deployments_url")]
    public string DeploymentsUrl { get; set; } = string.Empty;
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
    [JsonPropertyName("pushed_at")]
    public DateTimeOffset PushedAt { get; set; }
    [JsonPropertyName("git_url")]
    public string GitUrl { get; set; } = string.Empty;
    [JsonPropertyName("ssh_url")]
    public string SshUrl { get; set; } = string.Empty;
    [JsonPropertyName("clone_url")]
    public string CloneUrl { get; set; } = string.Empty;
    [JsonPropertyName("svn_url")]
    public string SvnUrl { get; set; } = string.Empty;
    public string Homepage { get; set; } = string.Empty;
    public long Size { get; set; }
    [JsonPropertyName("stargazers_count")]
    public long StargazersCount { get; set; }
    [JsonPropertyName("watchers_count")]
    public long WatchersCount { get; set; }
    public string Language { get; set; } = string.Empty;
    [JsonPropertyName("has_issues")]
    public bool HasIssues { get; set; }
    [JsonPropertyName("has_projects")]
    public bool HasProjects { get; set; }
    [JsonPropertyName("has_downloads")]
    public bool HasDownloads { get; set; }
    [JsonPropertyName("has_wiki")]
    public bool HasWiki { get; set; }
    [JsonPropertyName("has_pages")]
    public bool HasPages { get; set; }
    [JsonPropertyName("has_discussions")]
    public bool HasDiscussions { get; set; }
    [JsonPropertyName("forks_count")]
    public long ForksCount { get; set; }
    [JsonPropertyName("mirror_url")]
    public string MirrorUrl { get; set; } = string.Empty;
    public bool Archived { get; set; }
    public bool Disabled { get; set; }
    [JsonPropertyName("open_issues_count")]
    public long OpenIssuesCount { get; set; }
    public required GithubLicense License { get; set; }
    [JsonPropertyName("allow_forking")]
    public bool AllowForking { get; set; }
    [JsonPropertyName("is_template")]
    public bool IsTemplate { get; set; }
    [JsonPropertyName("web_commit_signoff_required")]
    public bool WebCommitSignoffRequired { get; set; }
    public List<string> Topics { get; set; } = new List<string>();
    public string Visibility { get; set; } = string.Empty;
    public long Forks { get; set; }
    [JsonPropertyName("open_issues")]
    public long OpenIssues { get; set; }
    public long Watchers { get; set; }
    [JsonPropertyName("default_branch")]
    public string DefaultBranch { get; set; } = string.Empty;
    [JsonPropertyName("temp_clone_token")]
    public string TempCloneToken { get; set; } = string.Empty;
    [JsonPropertyName("network_count")]
    public long NetworkCount { get; set; }
    [JsonPropertyName("subscribers_count")]
    public long SubscribersCount { get; set; }
    public Source? source { get; set; }

    public override string ToString()
    {
      return $"Package name: {Name}\n " +
        $"Package url:{HtmlUrl}\n" +
        $"Description: {Description}\n" +
        (Fork ? $"Warning: forked from: {source!.ToString()}!\n" : "") +
        $"Open issues and PRs: {OpenIssuesCount}\n" +
        $"Stars: {StargazersCount}\n" +
        $"Watchers: {SubscribersCount}\n" +
        $"Forks: {ForksCount}\n" +
        $"{License.ToString()}\n" +
        $"{Owner.ToString()}\n" +
        $"Created at: {CreatedAt.ToString()}\n" +
        $"Updated at: {UpdatedAt.ToString()}\n" +
        $"Pushed at: {PushedAt.ToString()}\n" +
        $"Homepage: {Homepage}\n" +
        $"Size: {Size}\n";
    }

    public static string GetGithubApiUrl(string RepositoryUrl)
    {
      var githubApiUrl = RepositoryUrl.Replace("https://", "https://api.").Replace("github.com", "github.com/repos");
      if(githubApiUrl.Split(".").Last() == "git")
      {
        githubApiUrl = githubApiUrl.Remove(githubApiUrl.Length - ".git".Length, ".git".Length);
      }
      return githubApiUrl;
    }

  }

  public class GithubOwner
  {
    public string Login { get; set; } = string.Empty;
    public long Id { get; set; }
    [JsonPropertyName("node_id")]
    public string NodeId { get; set; } = string.Empty;
    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; } = string.Empty;
    [JsonPropertyName("gravatar_id")]
    public string GravatarId { get; set; } = string.Empty;
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
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("site_admin")]
    public bool SiteAdmin { get; set; }

    public override string ToString()
    {
      return $"Owner name: {Login}\nOwner url: {HtmlUrl}";
    }
  }

  public class GithubLicense
  {
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("spdx_id")]
    public string SpdxId { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    [JsonPropertyName("node_id")]
    public string NodeId { get; set; } = string.Empty;

    public override string ToString()
    {
      if(Name=="Other" & Url == null){
        return $"Warning: Standard license not found!";
      }
      return $"License name: {Name}\nLicense url: {Url}";
    }
  }

  public class Source
  {
    public long Id { get; set; }
    [JsonPropertyName("node_id")]
    public string NodeId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = string.Empty;
    public bool Private { get; set; }
    public required GithubOwner Owner { get; set; }
    
    public override string ToString()
    {
      return $"{FullName} with owner {Owner.Login}";
    }
    
  }
}
