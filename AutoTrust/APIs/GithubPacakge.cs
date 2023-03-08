using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

namespace AutoTrust
{
  public class GithubPackage
  {
    public long Id { get; set; }
    public string NodeId { get; set; }
    public string Name { get; set; }
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
    public bool Private { get; set; }
    public GithubOwner Owner { get; set; }
    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; }
    public string Description { get; set; }
    public bool Fork { get; set; }
    public string Url { get; set; }
    [JsonPropertyName("forks_url")]
    public string ForksUrl { get; set; }
    [JsonPropertyName("keys_url")]
    public string KeysUrl { get; set; }
    [JsonPropertyName("collaborators_url")]
    public string CollaboratorsUrl { get; set; }
    [JsonPropertyName("teams_url")]
    public string TeamsUrl { get; set; }
    [JsonPropertyName("hooks_url")]
    public string HooksUrl { get; set; }
    [JsonPropertyName("issue_events_url")]
    public string IssueEventsUrl { get; set; }
    [JsonPropertyName("events_url")]
    public string EventsUrl { get; set; }
    [JsonPropertyName("assignees_url")]
    public string AssigneesUrl { get; set; }
    [JsonPropertyName("branches_url")]
    public string BranchesUrl { get; set; }
    [JsonPropertyName("tags_url")]
    public string TagsUrl { get; set; }
    [JsonPropertyName("blobs_url")]
    public string BlobsUrl { get; set; }
    [JsonPropertyName("git_tags_url")]
    public string GitTagsUrl { get; set; }
    [JsonPropertyName("git_refs_url")]
    public string GitRefsUrl { get; set; }
    [JsonPropertyName("trees_url")]
    public string TreesUrl { get; set; }
    [JsonPropertyName("statuses_url")]
    public string StatusesUrl { get; set; }
    [JsonPropertyName("languages_url")]
    public string LanguagesUrl { get; set; }
    [JsonPropertyName("stargazers_url")]
    public string StargazersUrl { get; set; }
    [JsonPropertyName("contributors_url")]
    public string ContributorsUrl { get; set; }
    [JsonPropertyName("subscribers_url")]
    public string SubscribersUrl { get; set; }
    [JsonPropertyName("subscription_url")]
    public string SubscriptionUrl { get; set; }
    [JsonPropertyName("commits_url")]
    public string CommitsUrl { get; set; }
    [JsonPropertyName("git_commits_url")]
    public string GitCommitsUrl { get; set; }
    [JsonPropertyName("comments_url")]
    public string CommentsUrl { get; set; }
    [JsonPropertyName("issue_comment_url")]
    public string IssueCommentUrl { get; set; }
    [JsonPropertyName("contents_url")]
    public string ContentsUrl { get; set; }
    [JsonPropertyName("compare_url")]
    public string CompareUrl { get; set; }
    [JsonPropertyName("merges_url")]
    public string MergesUrl { get; set; }
    [JsonPropertyName("archive_url")]
    public string ArchiveUrl { get; set; }
    [JsonPropertyName("downloads_url")]
    public string DownloadsUrl { get; set; }
    [JsonPropertyName("Issues_url")]
    public string IssuesUrl { get; set; }
    [JsonPropertyName("pulls_url")]
    public string PullsUrl { get; set; }
    [JsonPropertyName("milestones_url")]
    public string MilestonesUrl { get; set; }
    [JsonPropertyName("notifications_url")]
    public string NotificationsUrl { get; set; }
    [JsonPropertyName("labels_url")]
    public string LabelsUrl { get; set; }
    [JsonPropertyName("releases_url")]
    public string ReleasesUrl { get; set; }
    [JsonPropertyName("deployments_url")]
    public string DeploymentsUrl { get; set; }
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
    [JsonPropertyName("pushed_at")]
    public DateTimeOffset PushedAt { get; set; }
    [JsonPropertyName("git_url")]
    public string GitUrl { get; set; }
    [JsonPropertyName("ssh_url")]
    public string SshUrl { get; set; }
    [JsonPropertyName("clone_url")]
    public string CloneUrl { get; set; }
    [JsonPropertyName("svn_url")]
    public string SvnUrl { get; set; }
    public string Homepage { get; set; }
    public long Size { get; set; }
    [JsonPropertyName("stargazers_count")]
    public long StargazersCount { get; set; }
    [JsonPropertyName("watchers_count")]
    public long WatchersCount { get; set; }
    public string Language { get; set; }
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
    public string MirrorUrl { get; set; }
    public bool Archived { get; set; }
    public bool Disabled { get; set; }
    [JsonPropertyName("open_issues_count")]
    public long OpenIssuesCount { get; set; }
    public GithubLicense License { get; set; }
    [JsonPropertyName("allow_forking")]
    public bool AllowForking { get; set; }
    [JsonPropertyName("is_template")]
    public bool IsTemplate { get; set; }
    [JsonPropertyName("web_commit_signoff_required")]
    public bool WebCommitSignoffRequired { get; set; }
    public List<string> Topics { get; set; }
    public string Visibility { get; set; }
    public long Forks { get; set; }
    [JsonPropertyName("open_issues")]
    public long OpenIssues { get; set; }
    public long Watchers { get; set; }
    [JsonPropertyName("default_branch")]
    public string DefaultBranch { get; set; }
    [JsonPropertyName("temp_clone_token")]
    public string TempCloneToken { get; set; }
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
        (Fork ? $"Warning: forked from: {source.ToString()}!\n" : "") +
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

    public async static Task<GithubPackage?> GetGithubPackage(HttpClient httpClient, string repositoryUrl)
    {
      try
      {
        // Fetch package data
        var githubApiUrl = GithubPackage.GetGithubApiUrl(repositoryUrl);
        httpClient.DefaultRequestHeaders.Add("User-Agent", "request");
        var githubData = await httpClient.GetFromJsonAsync<GithubPackage>(githubApiUrl);
        return githubData;
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
    public string Login { get; set; }
    public long Id { get; set; }
    [JsonPropertyName("node_id")]
    public string NodeId { get; set; }
    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }
    [JsonPropertyName("gravatar_id")]
    public string GravatarId { get; set; }
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
    public string Type { get; set; }
    [JsonPropertyName("site_admin")]
    public bool SiteAdmin { get; set; }

    public override string ToString()
    {
      return $"Owner name: {Login}\nOwner url: {HtmlUrl}";
    }
  }

  public class GithubLicense
  {
    public string Key { get; set; }
    public string Name { get; set; }
    [JsonPropertyName("spdx_id")]
    public string SpdxId { get; set; }
    public string Url { get; set; }
    [JsonPropertyName("node_id")]
    public string NodeId { get; set; }

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
    public string NodeId { get; set; }
    public string Name { get; set; }
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
    public bool Private { get; set; }
    public GithubOwner Owner { get; set; }
    
    public override string ToString()
    {
      return $"{FullName} with owner {Owner.Login}";
    }
    
  }
}
