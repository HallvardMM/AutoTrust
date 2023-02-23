using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace AutoTrust
{
  public class GithubPackage
  {
    public long Id { get; set; }
    public string NodeId { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public bool Private { get; set; }
    public GithubOwner Owner { get; set; }
    public string HtmlUrl { get; set; }
    public string Description { get; set; }
    public bool Fork { get; set; }
    public string Url { get; set; }
    public string ForksUrl { get; set; }
    public string KeysUrl { get; set; }
    public string CollaboratorsUrl { get; set; }
    public string TeamsUrl { get; set; }
    public string HooksUrl { get; set; }
    public string IssueEventsUrl { get; set; }
    public string EventsUrl { get; set; }
    public string AssigneesUrl { get; set; }
    public string BranchesUrl { get; set; }
    public string TagsUrl { get; set; }
    public string BlobsUrl { get; set; }
    public string GitTagsUrl { get; set; }
    public string GitRefsUrl { get; set; }
    public string TreesUrl { get; set; }
    public string StatusesUrl { get; set; }
    public string LanguagesUrl { get; set; }
    public string StargazersUrl { get; set; }
    public string ContributorsUrl { get; set; }
    public string SubscribersUrl { get; set; }
    public string SubscriptionUrl { get; set; }
    public string CommitsUrl { get; set; }
    public string GitCommitsUrl { get; set; }
    public string CommentsUrl { get; set; }
    public string IssueCommentUrl { get; set; }
    public string ContentsUrl { get; set; }
    public string CompareUrl { get; set; }
    public string MergesUrl { get; set; }
    public string ArchiveUrl { get; set; }
    public string DownloadsUrl { get; set; }
    public string IssuesUrl { get; set; }
    public string PullsUrl { get; set; }
    public string MilestonesUrl { get; set; }
    public string NotificationsUrl { get; set; }
    public string LabelsUrl { get; set; }
    public string ReleasesUrl { get; set; }
    public string DeploymentsUrl { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset PushedAt { get; set; }
    public string GitUrl { get; set; }
    public string SshUrl { get; set; }
    public string CloneUrl { get; set; }
    public string SvnUrl { get; set; }
    public string Homepage { get; set; }
    public long Size { get; set; }
    public long StargazersCount { get; set; }
    public long WatchersCount { get; set; }
    public string Language { get; set; }
    public bool HasIssues { get; set; }
    public bool HasProjects { get; set; }
    public bool HasDownloads { get; set; }
    public bool HasWiki { get; set; }
    public bool HasPages { get; set; }
    public bool HasDiscussions { get; set; }
    public long ForksCount { get; set; }
    public string MirrorUrl { get; set; }
    public bool Archived { get; set; }
    public bool Disabled { get; set; }
    public long OpenIssuesCount { get; set; }
    public GithubLicense License { get; set; }
    public bool AllowForking { get; set; }
    public bool IsTemplate { get; set; }
    public bool WebCommitSignoffRequired { get; set; }
    public List<string> Topics { get; set; }
    public string Visibility { get; set; }
    public long Forks { get; set; }
    public long OpenIssues { get; set; }
    public long Watchers { get; set; }
    public string DefaultBranch { get; set; }
    public string TempCloneToken { get; set; }
    public long NetworkCount { get; set; }
    public long SubscribersCount { get; set; }

    public class GithubOwner
    {
      public string Login { get; set; }
      public long Id { get; set; }
      public string NodeId { get; set; }
      public string AvatarUrl { get; set; }
      public string GravatarId { get; set; }
      public string Url { get; set; }
      public string HtmlUrl { get; set; }
      public string FollowersUrl { get; set; }
      public string FollowingUrl { get; set; }
      public string GistsUrl { get; set; }
      public string StarredUrl { get; set; }
      public string SubscriptionsUrl { get; set; }
      public string OrganizationsUrl { get; set; }
      public string ReposUrl { get; set; }
      public string EventsUrl { get; set; }
      public string ReceivedEventsUrl { get; set; }
      public string Type { get; set; }
      public bool SiteAdmin { get; set; }
    }

    public class GithubLicense
    {
      public string Key { get; set; }
      public string Name { get; set; }
      public string SpdxId { get; set; }
      public string Url { get; set; }
      public string NodeId { get; set; }
    }

  }
}
